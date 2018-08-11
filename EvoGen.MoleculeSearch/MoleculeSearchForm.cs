﻿using EvoGen.Domain.Collections;
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
        private List<Thread> ThreadList;
        private Object queueObjectLock;
        private Object listObjectLock;

        private static volatile List<string> SearchList = new List<string>();
        private static volatile List<string> Ids = new List<string>();
        private static volatile int SearchCount = 0;
        private static volatile int DatabaseCount = 0;
        private static volatile Queue<MoleculeGraph> ResultQueue = new Queue<MoleculeGraph>();

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
            this.ThreadList = new List<Thread>();
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
                    var moleculeAtoms = FormulaGenerator.GenerateFormula();
                    var formula = FormulaGenerator.GetFormulaFromMolecule(moleculeAtoms);
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

        private int GetPopulationSize(Dictionary<string, int> molecule)
        {
            if (molecule.Count < 5)
                return 100;
            else if (molecule.Count >= 5 && molecule.Count < 7)
                return 500;
            else
                return 1000;
        }

        private int GetMaxGenerations(Dictionary<string, int> molecule)
        {
            var atomsCount = molecule.Sum(x => x.Value);
            if (atomsCount < 40)
                return 2000;
            else
                return 6000;
        }

        private double GetMutationRate(Dictionary<string, int> molecule)
        {
            return 0.20;
        }
    }
}
