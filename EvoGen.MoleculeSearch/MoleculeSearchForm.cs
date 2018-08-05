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
        private readonly IMoleculeService MleculeService;
        private bool Searching;
        private bool Saving;
        private List<Task> TaskList;
        private Queue<MoleculeGraph> resultQueue;
        private CustomRandom customRandom;

        public MoleculeSearchForm(IMoleculeService moleculeService)
        {
            InitializeComponent();

            this.MleculeService = moleculeService;
            this.Searching = false;
            this.Saving = false;
            this.TaskList = new List<Task>();
            this.timerSearch.Enabled = false;
            this.timerSave.Enabled = false;
            this.gridQueue.DataSource = new List<object>
            {
                new { Formula = "", IdStructure = "" }
            };
            this.resultQueue = new Queue<MoleculeGraph>();
            this.customRandom = new CustomRandom();
        }

        private void MoleculeSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Text = MleculeService.MoleculeCount().ToString();
            }
            catch (Exception)
            {
                txtQuantity.Text = "Erro de conexão";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Searching)
                Search();
            else
                StopSearch();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Saving)
                Save();
            else
                StopSave();
        }

        private void Search()
        {
            btnSearch.Text = "Parar Busca";
            btnSearch.BackColor = Color.Gray;
            Searching = true;
            timerSearch.Enabled = true;
            timerSearch.Start();
            new Task(() =>
            {
                while (Searching)
                {
                    if (resultQueue.Count > 0)
                    {
                        SetDataSource(gridQueue, resultQueue.ToList().Select(x => new
                        {
                            Formula = x.Nomenclature,
                            IdStructure = "Teste"
                        }).ToList());
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
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
            btnSave.Text = "Salvar de Salvar";
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

        private Dictionary<string, int> GenerateFormula()
        {
            var atomsList = Util.OoctetRule.Keys.ToList();
            var molecule = new Dictionary<string, int>();

            var totalAtomsMolecule = customRandom.NextTotalMoleculeAtoms();
            var totalDiferentAtomsMolecule = customRandom.NextDiferentMoleculeAtoms();

            var carbonRate = customRandom.NextDouble();
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
                    var removeAtom = atomsList[customRandom.Next(atomsList.Count)];
                    if (molecule.ContainsKey(removeAtom))
                        molecule.Remove(removeAtom);
                }
            }
            else
            {
                while (molecule.Count < totalDiferentAtomsMolecule)
                {
                    var addAtom = atomsList[customRandom.Next(atomsList.Count)];
                    if (!molecule.ContainsKey(addAtom))
                        molecule.Add(addAtom, 0);
                }
            }
            var moleculeAtoms = molecule.Keys.ToList();
            while (molecule.Sum(x => x.Value) < totalAtomsMolecule)
            {
                var atom = moleculeAtoms[customRandom.Next(moleculeAtoms.Count)];
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

        public void SetDataSource(DataGridView gridView, object dataSource)
        {
            if (gridView.InvokeRequired)
                new Task(() => gridView.Invoke(new MethodInvoker(() => gridView.DataSource = dataSource))).Start();
            else
                new Task(() => gridView.DataSource = dataSource).Start();
        }

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            TaskList.RemoveAll(x => x.Status == TaskStatus.RanToCompletion);
            while (TaskList.Count < (Environment.ProcessorCount / 2))
            {
                TaskList.Add(new Task(() =>
                {
                    var molecule = GenerateFormula();
                    var ga = new StructureGenerator(
                        GetFormulaFromMolecule(molecule),
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
                            resultQueue.Enqueue(result);
                        }
                    }
                }));
            }
            TaskList.Where(x => x.Status == TaskStatus.Created).ToList().ForEach(x => x.Start());
        }

        private void timerSave_Tick(object sender, EventArgs e)
        {

        }
    }
}
