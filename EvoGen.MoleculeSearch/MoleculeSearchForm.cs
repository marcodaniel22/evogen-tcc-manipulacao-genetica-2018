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
        private int SearchCount;
        private int DatabaseCount;
        private List<Task> TaskList;
        private Thread WatcherThread;
        private Thread DatabaseSaveThread;
        private List<string> SearchFormulaList;
        private Queue<MoleculeGraph> ResultQueue;
        private CustomRandom CustomRandom;

        public MoleculeSearchForm(IMoleculeService moleculeService)
        {
            InitializeComponent();
            this.timerSearch.Enabled = false;
            this.timerSave.Enabled = false;
            this.gridQueue.DataSource = new List<object>
            {
                new { Formula = "", IdStructure = "" }
            };
            this.gridSearches.DataSource = new List<object>
            {
                new { Searches = "" }
            };

            this.MoleculeService = moleculeService;
            this.Searching = false;
            this.Saving = false;
            this.SearchCount = 0;
            this.DatabaseCount = 0;
            this.TaskList = new List<Task>();
            this.ResultQueue = new Queue<MoleculeGraph>();
            this.CustomRandom = new CustomRandom();
            this.SearchFormulaList = new List<string>();
        }

        private void MoleculeSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                new Task(() => DatabaseCount = MoleculeService.MoleculeCount()).Start();
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

            WatcherThread = new Thread(() =>
            {
                while (Searching)
                {
                    //if (ResultQueue.Count > 0)
                    //{
                    //    SetDataSource(gridQueue, ResultQueue.ToList().Select(x => new
                    //    {
                    //        Formula = x.Nomenclature,
                    //        IdStructure = "Teste"
                    //    }).ToList());
                    //}
                    if (SearchFormulaList.Count > 0)
                    {
                        SetDataSource(gridSearches, SearchFormulaList.Select(x => new
                        {
                            Searches = x
                        }).ToList());
                    }

                    SetText(txtProcess, TaskList.Count.ToString());
                    SetText(txtQuantityDatabase, DatabaseCount.ToString());
                    SetText(txtFound, SearchCount.ToString());

                    Thread.Sleep(1000);
                }
            });
            WatcherThread.Start();
        }

        private void StopSearch()
        {
            btnSearch.Text = "Iniciar Busca";
            btnSearch.BackColor = Color.Gainsboro;
            Searching = false;
            timerSearch.Enabled = false;
            timerSearch.Stop();

            if (WatcherThread != null && WatcherThread.IsAlive)
                WatcherThread.Abort();
            WatcherThread = null;
        }

        private void Save()
        {
            btnSave.Text = "Salvar de Salvar";
            btnSave.BackColor = Color.Gray;
            Saving = true;
            timerSave.Enabled = true;
            timerSave.Start();

            DatabaseSaveThread = new Thread(() =>
            {
                while (Saving)
                {
                    Thread.Sleep(1000);
                }
            });
            DatabaseSaveThread.Start();
        }

        private void StopSave()
        {
            btnSave.Text = "Salvar em Banco";
            btnSave.BackColor = Color.Gainsboro;
            Saving = false;
            timerSave.Enabled = false;
            timerSave.Stop();

            if (DatabaseSaveThread != null && DatabaseSaveThread.IsAlive)
                DatabaseSaveThread.Abort();
            DatabaseSaveThread = null;
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
                {
                    nomenclature += String.Format("{0}{1}", atom, molecule[atom] > 1 ? molecule[atom].ToString() : "");
                }
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

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            var finalizedTasks = TaskList.Where(x => x.Status == TaskStatus.RanToCompletion).ToList();
            finalizedTasks.ForEach(x =>
            {
                x.Dispose();
                TaskList.Remove(x);
            });

            while (TaskList.Count < (Environment.ProcessorCount))
            //while (TaskList.Count < (1))
            {
                TaskList.Add(new Task(() =>
                {
                    var molecule = GenerateFormula();
                    var formula = GetFormulaFromMolecule(molecule);
                    if (!string.IsNullOrEmpty(formula))
                    {
                        SearchFormulaList.Add(formula);
                        var ga = new StructureGenerator(
                            formula,
                            GetPopulationSize(molecule),
                            GetMaxGenerations(molecule),
                            GetMutationRate(molecule)
                        );
                        new Task(() => ga.FindSolutions()).Start();
                        while (!ga.Finished)
                        {
                            if (ga.ResultList.Count > 0)
                            {
                                var result = ga.ResultList.Dequeue();
                                ResultQueue.Enqueue(result);
                            }
                        }
                        SearchFormulaList.Remove(formula);
                        SearchFormulaList.RemoveAll(x => string.IsNullOrEmpty(x));
                    }
                }));
            }
            TaskList.Where(x => x.Status == TaskStatus.Created).ToList().ForEach(x => x.Start());
        }

        private void timerSave_Tick(object sender, EventArgs e)
        {

        }

        public void SetDataSource(DataGridView gridView, object dataSource)
        {
            if (gridView.InvokeRequired)
                new Task(() => gridView.Invoke(new MethodInvoker(() => gridView.DataSource = dataSource))).Start();
            else
                new Task(() => gridView.DataSource = dataSource).Start();
        }

        public void SetText(TextBox txtBox, string texto)
        {
            if (txtBox.InvokeRequired)
                new Task(() => txtBox.Invoke(new MethodInvoker(() => txtBox.Text = texto))).Start();
            else
                new Task(() => txtBox.Text = texto).Start();
        }
    }
}
