using EvoGen.Domain.Collections;
using EvoGen.Domain.DataGen;
using EvoGen.Domain.GA.StructureGenerator;
using EvoGen.Domain.Interfaces.Services;
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
        private readonly IMoleculeService _moleculeService;
        private readonly ILogService _logService;
        private readonly ILinkService _linkService;

        private bool Searching;
        private bool Saving;
        private bool FromRandom;
        private bool FromEmpty;

        private List<Thread> ThreadSearch;
        private Thread ThreadSave;
        private DateTime ThreadSaveInit;

        private Object queueObjectLock;
        private Object listObjectLock;

        private static volatile List<string> SearchList = new List<string>();
        private static volatile List<string> Ids = new List<string>();
        private static volatile int SearchCount = 0;
        private static volatile int DatabaseCount = 0;
        private static volatile Queue<MoleculeGraph> ResultQueue = new Queue<MoleculeGraph>();
        private static volatile bool SavingDataBase = false;

        public MoleculeSearchForm(IMoleculeService moleculeService, ILogService logService, ILinkService linkService)
        {
            InitializeComponent();
            this.timerSearch.Enabled = false;
            this.timerSave.Enabled = false;
            this.gridQueue.DataSource = new List<object>();
            this.gridSearches.DataSource = new List<object>();

            this._moleculeService = moleculeService;
            this._logService = logService;
            this._linkService = linkService;

            this.Searching = false;
            this.Saving = false;
            this.FromRandom = false;
            this.FromEmpty = true;

            this.ThreadSearch = new List<Thread>();
            this.queueObjectLock = new object();
            this.listObjectLock = new object();
        }

        private void MoleculeSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                new Task(() =>
                {
                    DatabaseCount = _moleculeService.GetMoleculeCount();
                    SetText(txtQuantityDatabase, DatabaseCount.ToString());
                }).Start();
            }
            catch (Exception)
            {
                txtQuantityDatabase.Text = "Erro de conexão";
            }
        }

        private void rbEmpty_CheckedChanged(object sender, EventArgs e)
        {
            FromEmpty = rbEmpty.Checked;
        }

        private void rbRandom_CheckedChanged(object sender, EventArgs e)
        {
            FromRandom = rbRandom.Checked;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Searching)
            {
                Searching = true;
                SetButtonStyle(btnSearch, "Parar Busca", Color.Gray);
                rbRandom.Enabled = false;
                rbEmpty.Enabled = false;
                timerSearch.Enabled = true;
                timerSearch.Start();
            }
            else
            {
                Searching = false;
                SetButtonStyle(btnSearch, "Iniciar Busca", Color.Gainsboro);
                rbRandom.Enabled = true;
                rbEmpty.Enabled = true;
                timerSearch.Enabled = false;
                timerSearch.Stop();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Saving)
            {
                Saving = true;
                SetButtonStyle(btnSave, "Parar de Salvar", Color.Gray);
                timerSave.Enabled = true;
                timerSave.Start();
            }
            else
            {
                Saving = false;
                SetButtonStyle(btnSave, "Salvar em Banco", Color.Gainsboro);
                timerSave.Enabled = false;
                timerSave.Stop();
            }
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            if (ThreadSearch.Count < (Environment.ProcessorCount / 2))
            //if (ThreadSearch.Count < (1))
            {
                ThreadSearch.Add(new Thread(() =>
                {
                    string formula = string.Empty;
                    int atomsCount = 0;
                    int diferentAtomsCount = 0;
                    int searchCounter = 0;
                    bool fromDataSet = false;
                    FormulaGenerator fg = new FormulaGenerator();
                    try
                    {
                        if (FromEmpty)
                        {
                            var molecule = _moleculeService.GetRandomToSearch(2, 50);
                            formula = molecule.Nomenclature;
                            atomsCount = molecule.AtomsCount;
                            diferentAtomsCount = molecule.DiferentAtomsCount;
                            fromDataSet = molecule.FromDataSet;
                            searchCounter = _logService.GetCounter(formula);
                        }
                        else if (FromRandom)
                        {
                            var moleculeAtoms = fg.GenerateFormula();
                            formula = fg.GetFormulaFromMolecule(moleculeAtoms);
                            atomsCount = moleculeAtoms.Sum(x => x.Value);
                            diferentAtomsCount = moleculeAtoms.Count;
                            searchCounter = _logService.GetCounter(formula);
                        }

                        if (!string.IsNullOrEmpty(formula))
                        {
                            if (fromDataSet)
                                _logService.NewSearch(formula);
                            lock (listObjectLock)
                            {
                                SearchList.Add(formula);
                                ShowSearchDataSource();
                            }
                            var ga = new StructureGenerator(
                                formula,
                                GetPopulationSize(atomsCount, diferentAtomsCount, searchCounter),
                                GetMaxGenerations(atomsCount, diferentAtomsCount, searchCounter),
                                GetMutationRate(atomsCount, diferentAtomsCount, searchCounter)
                            );
                            new Task(() => ga.FindSolutions()).Start();
                            string idStructure = string.Empty;
                            int resultCounter = 0;
                            while (!ga.Finished)
                            {
                                if (ga.ResultList.Count > 0)
                                {
                                    var molecule = ga.ResultList.Dequeue();
                                    if (molecule != null)
                                    {
                                        resultCounter++;
                                        molecule.ReorganizeLinks();
                                        molecule.SetEnergy();
                                        molecule.FromDataSet = fromDataSet;
                                        idStructure = _linkService.GetIdStructure(molecule.LinkEdges);
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
                            if (!fromDataSet && resultCounter > 0)
                                _logService.NewSearch(formula);
                            Ids.RemoveAll(x => x == idStructure);

                            lock (listObjectLock)
                            {
                                SearchList.RemoveAll(x => x == formula);
                                ShowSearchDataSource();
                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (SearchList.Contains(formula))
                            SearchList.RemoveAll(x => x == formula);
                        ShowSearchDataSource();
                    }
                }));
            }

            ThreadSearch.Where(x => x.ThreadState == ThreadState.Unstarted).ToList().ForEach(x => x.Start());
            var deleteThreads = ThreadSearch.Where(x => !x.IsAlive).ToList();
            foreach (var thread in deleteThreads)
                ThreadSearch.Remove(thread);

            txtProcess.Text = ThreadSearch.Count.ToString();
            txtQuantityDatabase.Text = DatabaseCount.ToString();
            txtFound.Text = SearchCount.ToString();
        }

        private void timerSave_Tick(object sender, EventArgs e)
        {
            if (!SavingDataBase)
            {
                SavingDataBase = true;
                MoleculeGraph molecule = null;
                Molecule saved = null;
                try
                {
                    if (ResultQueue.Count > 0)
                    {
                        lock (queueObjectLock)
                        {
                            molecule = ResultQueue.Dequeue();
                            ShowQueueDataSource();
                        }

                        if (molecule != null && _moleculeService.GetByIdStructure(molecule.Nomenclature, molecule.IdStructure) == null)
                        {
                            if (!string.IsNullOrEmpty(molecule.IdStructure))
                                saved = _moleculeService.Create(molecule);

                            var emptyMolecule = _moleculeService.GetByIdStructure(molecule.Nomenclature, null);
                            if (emptyMolecule != null && _moleculeService.GetNotEmptyMoleculeCount(molecule.Nomenclature) > 0)
                                _moleculeService.Delete(emptyMolecule);
                        }

                        if (saved != null)
                        {
                            SearchCount++;
                            DatabaseCount++;
                        }
                    }
                    DatabaseCount = _moleculeService.GetMoleculeCount();
                }
                catch (Exception)
                {
                    lock (queueObjectLock)
                    {
                        if (molecule != null && saved == null)
                            ResultQueue.Enqueue(molecule);
                        ShowQueueDataSource();
                    }
                }
                finally
                {
                    SavingDataBase = false;
                }
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

        public void SetButtonStyle(Button button, string label, Color color)
        {
            if (button.InvokeRequired)
                new Task(() => button.Invoke(new MethodInvoker(() =>
                {
                    button.Text = label;
                    button.BackColor = color;
                }))).Start();
            else
            {
                button.Text = label;
                button.BackColor = color;
            }
        }

        public void SetButtonEnabled(Button button, bool enabled)
        {
            if (button.InvokeRequired)
                new Task(() => button.Invoke(new MethodInvoker(() => button.Enabled = enabled))).Start();
            else
                new Task(() => button.Enabled = enabled).Start();
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
                gridView.DataSource = dataSource;
        }

        private int GetPopulationSize(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            var result = 0;
            if (atomsCount < 5)
                result = 100;
            else if (atomsCount >= 5 && atomsCount < 7)
                result = 150;
            else
                result = 200;
            return result + Convert.ToInt32(result * (searchCounter + 1) * 0.1);
        }

        private int GetMaxGenerations(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            var result = 0;
            if (atomsCount < 40)
                result = 2000;
            else
                result = 4000;
            return (result * (searchCounter + 1));
        }

        private double GetMutationRate(int atomsCount, int diferentAtomsCount, int searchCounter)
        {
            if ((searchCounter % 2) == 0)
                return 0.20;
            return 0.15;
        }
    }
}
