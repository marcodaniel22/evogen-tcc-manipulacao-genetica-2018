using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Services;
using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvoGen.MoleculeSearch
{
    public partial class MoleculeSearchForm : Form
    {
        private readonly IMoleculeService MoleculeService;
        private bool Searching;
        private bool Saving;
        private bool SavingMolecules;
        private int SearchCount;
        private int DatabaseCount;
        private List<Thread> ThreadList;
        private Queue<MoleculeGraph> ResultQueue;
        private List<string> SearchList;
        private CustomRandom CustomRandom;
        private List<string> Ids;
        private Object queueObjectLock;
        private Object listObjectLock;

        public MoleculeSearchForm(IMoleculeService moleculeService)
        {
            InitializeComponent();
            this.timerSearch.Enabled = false;
            this.timerSave.Enabled = false;
            this.gridQueue.DataSource = new List<object>();
            this.gridSearches.DataSource = new List<object>();

            this.MoleculeService = moleculeService;
            this.Searching = false;
            this.Saving = false;
            this.SearchCount = 0;
            this.DatabaseCount = 0;
            this.ThreadList = new List<Thread>();
            this.ResultQueue = new Queue<MoleculeGraph>();
            this.SearchList = new List<string>();
            this.CustomRandom = new CustomRandom();
            this.Ids = new List<string>();
            this.queueObjectLock = new object();
            this.listObjectLock = new object();
        }

        private void MoleculeSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                new Task(() =>
                {
                    DatabaseCount = MoleculeService.MoleculeCount();
                    SetText(txtQuantityDatabase, DatabaseCount.ToString());
                }).Start();
            }
            catch (Exception)
            {
                txtQuantityDatabase.Text = "Erro de conexão";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Searching) Search();
            else StopSearch();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Saving) Save();
            else StopSave();
        }

        private void Search()
        {
            btnSearch.Text = "Parar Busca";
            btnSearch.BackColor = Color.Gray;
            Searching = true;
            timerSearch.Enabled = true;
            timerSearch.Start();
        }

        private void StopSearch()
        {
            btnSearch.Text = "Iniciar Busca";
            btnSearch.BackColor = Color.Gainsboro;
            Searching = false;
            timerSearch.Enabled = false;
            timerSearch.Stop();
        }

        private void Save()
        {
            btnSave.Text = "Parar de Salvar";
            btnSave.BackColor = Color.Gray;
            Saving = true;
            timerSave.Enabled = true;
            timerSave.Start();
        }

        private void StopSave()
        {
            btnSave.Text = "Salvar em Banco";
            btnSave.BackColor = Color.Gainsboro;
            Saving = false;
            timerSave.Enabled = false;
            timerSave.Stop();
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (ThreadList.Count < (Environment.ProcessorCount / 2))
            //while (TaskList.Count < (1))
            {
                ThreadList.Add(new Thread(() =>
                {
                    var moleculeAtoms = GenerateFormula();
                    var formula = GetFormulaFromMolecule(moleculeAtoms);
                    if (!string.IsNullOrEmpty(formula))
                    {
                        lock (listObjectLock)
                        {
                            SearchList.Add(formula);
                            ShowSearchDataSource();
                        }
                        var ga = new StructureGenerator(
                            formula,
                            GetPopulationSize(moleculeAtoms),
                            GetMaxGenerations(moleculeAtoms),
                            GetMutationRate(moleculeAtoms)
                        );
                        new Task(() => ga.FindSolutions()).Start();
                        string idStructure = string.Empty;
                        while (!ga.Finished)
                        {
                            if (ga.ResultList.Count > 0)
                            {
                                var molecule = ga.ResultList.Dequeue();
                                if (molecule != null)
                                {
                                    idStructure = MoleculeGraph.GetIdStructure(molecule.LinkEdges);
                                    molecule.IdStructure = idStructure;
                                    if (!Ids.Contains(molecule.IdStructure))
                                    {
                                        Ids.Add(molecule.IdStructure);
                                        lock (queueObjectLock)
                                        {
                                            ResultQueue.Enqueue(molecule);
                                            ShowQueueDataSource();
                                        }
                                    }
                                }
                            }
                        }
                        lock (listObjectLock)
                        {
                            Ids.RemoveAll(x => x == idStructure);
                            SearchList.RemoveAll(x => x == formula);
                            ShowSearchDataSource();
                        }
                    }
                }));
            }

            ThreadList.Where(x => x.ThreadState == ThreadState.Unstarted).ToList().ForEach(x => x.Start());
            ThreadList.RemoveAll(x => !x.IsAlive);

            txtProcess.Text = ThreadList.Count.ToString();
            txtQuantityDatabase.Text = DatabaseCount.ToString();
            txtFound.Text = SearchCount.ToString();
        }

        private void timerSave_Tick(object sender, EventArgs e)
        {
            if (!SavingMolecules)
            {
                SavingMolecules = true;
                new Task(() =>
                {
                    while (ResultQueue.Count > 0)
                    {
                        MoleculeGraph molecule;
                        lock (queueObjectLock)
                        {
                            molecule = ResultQueue.Dequeue();
                            ShowQueueDataSource();
                        }
                        var tries = 3;
                        Molecule saved = null;
                        do
                        {
                            try
                            {
                                if (molecule != null && MoleculeService.GetByIdStructure(molecule.Nomenclature, molecule.IdStructure) == null)
                                    saved = MoleculeService.Create(molecule);
                            }
                            catch (Exception) { }
                        } while (--tries > 0 && saved == null);
                        if (saved != null)
                        {
                            SearchCount++;
                            DatabaseCount++;
                        }
                    }
                    SavingMolecules = false;
                }).Start();
            }
        }

        private void ShowQueueDataSource()
        {
            SetDataSource(gridQueue, ResultQueue.ToList().Select(x => new
            {
                Formula = x.Nomenclature,
                IdStructure = x.IdStructure
            }).ToList());
        }

        private void ShowSearchDataSource()
        {
            SetDataSource(gridSearches, SearchList.Select(x => new
            {
                Searches = x
            }).ToList());
        }

        public void SetText(TextBox lblText, string texto)
        {
            if (lblText.InvokeRequired)
                new Task(() => lblText.Invoke(new MethodInvoker(() => lblText.Text = texto))).Start();
            else
                new Task(() => lblText.Text = texto).Start();
        }

        public void SetDataSource(DataGridView gridView, object dataSource)
        {
            if (gridView.InvokeRequired)
                new Task(() => gridView.Invoke(new MethodInvoker(() => gridView.DataSource = dataSource))).Start();
            else
                new Task(() => gridView.DataSource = dataSource).Start();
        }

        private Dictionary<string, int> GenerateFormula()
        {
            var atomsList = Util.OoctetRule.Keys.ToList();
            var molecule = new Dictionary<string, int>();

            var totalAtomsMolecule = CustomRandom.NextTotalMoleculeAtoms();
            var totalDiferentAtomsMolecule = CustomRandom.NextDiferentMoleculeAtoms();

            var carbonRate = CustomRandom.NextDouble();
            if (carbonRate < 0.80)
                molecule.Add("C", 1);

            if (totalDiferentAtomsMolecule > (atomsList.Count / 2))
            {
                foreach (var atom in atomsList)
                {
                    if (!molecule.ContainsKey(atom))
                        molecule.Add(atom, 0);
                }
                while (molecule.Count > totalDiferentAtomsMolecule)
                {
                    var removeAtom = atomsList[CustomRandom.Next(atomsList.Count)];
                    if (molecule.ContainsKey(removeAtom))
                        molecule.Remove(removeAtom);
                }
            }
            else
            {
                while (molecule.Count < totalDiferentAtomsMolecule)
                {
                    var addAtom = atomsList[CustomRandom.Next(atomsList.Count)];
                    if (!molecule.ContainsKey(addAtom))
                        molecule.Add(addAtom, 0);
                }
            }
            var moleculeAtoms = molecule.Keys.ToList();
            while (molecule.Sum(x => x.Value) < totalAtomsMolecule)
            {
                var atom = moleculeAtoms[CustomRandom.Next(moleculeAtoms.Count)];
                molecule[atom] = (molecule[atom] + 1);
            }
            var removeAtoms = molecule.Where(x => x.Value == 0).ToList();
            foreach (var removeAtom in removeAtoms)
            {
                molecule.Remove(removeAtom.Key);
            }
            return molecule;
        }

        private string GetFormulaFromMolecule(Dictionary<string, int> molecule)
        {
            var atomsList = Util.OoctetRule.Keys.ToList();
            var nomenclature = String.Empty;
            foreach (var atom in atomsList)
            {
                if (molecule.ContainsKey(atom))
                    nomenclature += String.Format("{0}{1}", atom, molecule[atom] > 1 ? molecule[atom].ToString() : "");
            }
            return nomenclature;
        }

        private int GetPopulationSize(Dictionary<string, int> molecule)
        {
            return 100;
        }

        private int GetMaxGenerations(Dictionary<string, int> molecule)
        {
            return 2000;
        }

        private double GetMutationRate(Dictionary<string, int> molecule)
        {
            return 0.20;
        }
    }
}
