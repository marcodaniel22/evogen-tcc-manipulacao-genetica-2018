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
using System.Windows.Forms.DataVisualization.Charting;

namespace EvoGen.MoleculeValidation
{
    public partial class MoleculeValidationForm : Form
    {
        private Thread searchThread;
        private volatile GA ga;
        private volatile int timerCounter;

        public MoleculeValidationForm()
        {
            InitializeComponent();

            chart1.Series.Add(new Series("Fitness"));

            chart1.Series["Fitness"].ChartType = SeriesChartType.Line;
            chart1.Series["Fitness"].Color = Color.Blue;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
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
                this.chart1.Series[0].Points.Clear();
                var populationSize = Int32.Parse(this.txtPopulationSize.Text);
                var maxGenerations = Int32.Parse(this.txtMaxGenerations.Text);
                var mutationRate = Double.Parse(this.txtMutationRate.Text);
                var nomenclature = this.txtNomenclature.Text;

                this.timerCounter = 0;
                this.timer1.Start();
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
                    while (searchThread != null && searchThread.IsAlive)
                    {
                        Thread.Sleep(500);
                    }
                    this.SetEnabled(this.btnCancel, false);
                    this.SetEnabled(this.btnSearch, true);
                    this.timer1.Enabled = false;
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
                        var bestFitness = this.ga.BestIndividual.Fitness;
                        this.SetText(this.lblBestFitness, bestFitness.ToString());
                        this.SetChartSerie(this.chart1, Int32.Parse(this.lblGenerations.Text), bestFitness);
                        Thread.Sleep(500);
                    }
                }
            }).Start();
        }


        private void CancelSearch()
        {
            try
            {
                if (this.timer1.Enabled)
                    this.timer1.Stop();
                if (this.searchThread != null && this.searchThread.IsAlive)
                    this.searchThread.Abort();
                this.searchThread = null;
                this.ga = null;
                this.lblStopWatch.Text = "0.00";
                this.lblGenerations.Text = "0";
                this.lblBestFitness.Text = "0";
                this.timerCounter = 0;
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

        private void SetChartSerie(Chart chart, int v, double bestFitness)
        {
            if (chart.InvokeRequired)
                new Task(() => chart.Invoke(new MethodInvoker(() => chart.Series["Fitness"].Points.AddXY(v, bestFitness)))).Start();
            else
                new Task(() => chart.Series["Fitness"].Points.AddXY(v, bestFitness)).Start();
        }

        public void SetStatus(string message)
        {
            new Task(() => this.SetText(this.lblStatus, message)).Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.timer1.Enabled)
            {
                new Task(() =>
                {
                    this.timerCounter++;
                    var minutes = 0;
                    while ((this.timerCounter - (minutes * 60)) >= 60)
                    {
                        minutes++;
                    }
                    var seconds = this.timerCounter - (minutes * 60);
                    this.SetText(this.lblStopWatch, string.Format("{0}:{1}{2}", minutes, (seconds < 10 ? "0" : ""), seconds));
                }).Start();
            }
        }
    }
}
