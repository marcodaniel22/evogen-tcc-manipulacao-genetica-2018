using EvoGen.Domain.Collections;
using EvoGen.Domain.GA.StructureGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EvoGen.MoleculeValidation
{
    public partial class MoleculeValidationForm : Form
    {
        private Thread searchThread;
        private volatile StructureGenerator ga;
        private volatile int timerCounter;

        public MoleculeValidationForm()
        {
            InitializeComponent();

            chart1.Series.Add(new Series("Maior Fitness"));
            chart1.Series.Add(new Series("Menor Fitness"));

            chart1.Series["Menor Fitness"].ChartType = SeriesChartType.Line;
            chart1.Series["Menor Fitness"].Color = Color.Blue;
            chart1.Series["Maior Fitness"].ChartType = SeriesChartType.Line;
            chart1.Series["Maior Fitness"].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnSearch.Enabled = false;
            StartSearch();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;
            CancelSearch();
        }

        private void StartSearch()
        {
            try
            {
                CancelSearch();
                ClearCharts();

                var populationSize = Int32.Parse(this.txtPopulationSize.Text);
                var maxGenerations = Int32.Parse(this.txtMaxGenerations.Text);
                var mutationRate = Double.Parse(this.txtMutationRate.Text);
                var nomenclature = this.txtNomenclature.Text;

                StartTimer();
                this.searchThread = new Thread(() =>
                {
                    SetStatus("Iniciando população...");
                    ga = new StructureGenerator(nomenclature, populationSize, maxGenerations, mutationRate);
                    StartWatchers();
                    SetStatus("Procurando estrutura molecular...");
                    MoleculeGraph molecule = ga.FindSolution();

                    SetDataSource(gridResult, molecule.LinkEdges.Select(x => new
                    {
                        From = x.From.ToString(),
                        To = x.To.ToString()
                    }).ToList());

                    SetStatus("Fim!");
                });
                searchThread.Start();

                new Task(() =>
                {
                    while (searchThread != null && searchThread.IsAlive)
                        Thread.Sleep(500);

                    SetEnabled(btnCancel, false);
                    SetEnabled(btnSearch, true);
                    timer1.Enabled = false;
                }).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void ClearCharts()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
        }

        public void StartWatchers()
        {
            new Task(() =>
            {
                while (ga != null)
                {
                    SetText(lblGenerations, ga.Generation.ToString());
                    Thread.Sleep(100);
                }
            }).Start();
            new Task(() =>
            {
                while (ga != null)
                {
                    if (ga.BestIndividual != null && ga.WorseIndividual != null)
                    {
                        var bestFitness = ga.BestIndividual.Fitness;
                        var worstFitness = ga.WorseIndividual.Fitness;
                        SetText(lblBestFitness, bestFitness.ToString());
                        SetChartSerie(chart1, Int32.Parse(lblGenerations.Text), bestFitness, "Menor Fitness");
                        SetChartSerie(chart1, Int32.Parse(lblGenerations.Text), worstFitness, "Maior Fitness");
                        Thread.Sleep(1000);
                    }
                }
            }).Start();
        }


        private void CancelSearch()
        {
            try
            {
                StopTimer();
                CancelThread();
                ResetVariables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void CancelThread()
        {
            if (searchThread != null && searchThread.IsAlive)
                searchThread.Abort();
            searchThread = null;
        }

        private void StartTimer()
        {
            timerCounter = 0;
            timer1.Start();
        }

        private void StopTimer()
        {
            if (timer1.Enabled)
                timer1.Stop();
        }

        private void ResetVariables()
        {
            ga = null;
            lblStopWatch.Text = "0.00";
            lblGenerations.Text = "0";
            lblBestFitness.Text = "0";
            timerCounter = 0;
            gridResult.DataSource = new List<object>
            {
                new { From = "", To = "" }
            };
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

        private void SetChartSerie(Chart chart, int v, double bestFitness, string lineChart)
        {
            if (chart.InvokeRequired)
                new Task(() => chart.Invoke(new MethodInvoker(() => chart.Series[lineChart].Points.AddXY(v, bestFitness)))).Start();
            else
                new Task(() => chart.Series[lineChart].Points.AddXY(v, bestFitness)).Start();
        }

        public void SetStatus(string message)
        {
            new Task(() => SetText(lblStatus, message)).Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                new Task(() =>
                {
                    timerCounter++;
                    var minutes = 0;
                    while ((timerCounter - (minutes * 60)) >= 60)
                    {
                        minutes++;
                    }
                    var seconds = timerCounter - (minutes * 60);
                    SetText(lblStopWatch, string.Format("{0}:{1}{2}", minutes, (seconds < 10 ? "0" : ""), seconds));
                }).Start();
            }
        }

        private void MoleculeValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelSearch();
        }
    }
}
