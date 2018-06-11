using EvoGen.GA_MoleculeValidation;
using EvoGen.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvoGen.MoleculeValidation
{
    public partial class MoleculeValidationForm : Form
    {
        private volatile GA ga;
        private Thread searchThread;

        public MoleculeValidationForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = true;
            this.btnSearch.Enabled = false;
            this.StartSearch();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.btnSearch.Enabled = true;
            this.CancelSearch();
        }

        private void StartSearch()
        {
            try
            {
                this.CancelSearch();
                var populationSize = Int32.Parse(this.txtPopulationSize.Text);
                var maxGenerations = Int32.Parse(this.txtMaxGenerations.Text);
                var mutationRate = Double.Parse(this.txtMutationRate.Text);
                var nomenclature = this.txtNomenclature.Text;

                this.searchThread = new Thread(() =>
                {
                    this.SetStatus("Initializing population...");
                    this.ga = new GA(nomenclature, populationSize, maxGenerations, mutationRate);
                    this.StartWatchers();
                    this.SetStatus("Searching molecule structures...");
                    MoleculeGraph molecule = this.ga.FindSolution();
                    this.SetDataSource(this.gridResult, molecule.LinkEdges.Select(x => new
                    {
                        From = x.From.ToString(),
                        To = x.To.ToString()
                    }).ToList());
                    this.SetStatus("Finished!");
                });
                searchThread.Start();
                new Task(() =>
                {
                    while(searchThread != null && searchThread.IsAlive)
                    {
                        Thread.Sleep(500);
                    }
                    this.SetEnabled(this.btnCancel, false);
                    this.SetEnabled(this.btnSearch, true);
                }).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void StartWatchers()
        {
            new Task(() =>
            {
                while (this.ga != null)
                {
                    this.SetText(this.lblGenerations, this.ga.Generation.ToString());
                    Thread.Sleep(100);
                }
            }).Start();
            new Task(() =>
            {
                while (this.ga != null)
                {
                    if (this.ga.BestIndividual != null)
                    {
                        this.SetText(this.lblBestFitness, this.ga.BestIndividual.Fitness.ToString());
                        Thread.Sleep(100);
                    }
                }
            }).Start();
        }

        private void CancelSearch()
        {
            try
            {
                if (this.searchThread != null && this.searchThread.IsAlive)
                    this.searchThread.Abort();
                this.searchThread = null;
                this.ga = null;
                this.lblStopWatch.Text = "0.00";
                this.lblGenerations.Text = "0";
                this.gridResult.DataSource = new
                {
                    From = new List<string>(),
                    To = new List<string>()
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void SetText(Label lblText, string texto)
        {
            if (lblText.InvokeRequired)
                new Task(() => lblText.Invoke(new MethodInvoker(() => lblText.Text = texto))).Start();
            else
                new Task(() => lblText.Text = texto).Start();
        }

        public void SetEnabled(Button btnEnabled, bool enabled)
        {
            if (btnEnabled.InvokeRequired)
                new Task(() => btnEnabled.Invoke(new MethodInvoker(() => btnEnabled.Enabled = enabled))).Start();
            else
                new Task(() => btnEnabled.Enabled = enabled).Start();
        }

        public void SetDataSource(DataGridView gridView, object dataSource)
        {
            if (gridView.InvokeRequired)
                new Task(() => gridView.Invoke(new MethodInvoker(() => gridView.DataSource = dataSource))).Start();
            else
                new Task(() => gridView.DataSource = dataSource).Start();
        }

        public void SetStatus(string message)
        {
            new Task(() => this.SetText(this.lblStatus, message)).Start();
        }
    }
}
