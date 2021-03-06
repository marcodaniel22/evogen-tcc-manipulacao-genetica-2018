﻿namespace EvoGen.MoleculeValidation
{
    partial class MoleculeValidationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoleculeValidationForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBestFitness = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblGenerations = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMaxGenerations = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStopWatch = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtMutationRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPopulationSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNomenclature = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridResult = new System.Windows.Forms.DataGridView();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label8 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBestFitness);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.lblGenerations);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtMaxGenerations);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblStopWatch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtMutationRate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPopulationSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNomenclature);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 289);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configurações do AG";
            // 
            // lblBestFitness
            // 
            this.lblBestFitness.AutoSize = true;
            this.lblBestFitness.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBestFitness.Location = new System.Drawing.Point(437, 247);
            this.lblBestFitness.Name = "lblBestFitness";
            this.lblBestFitness.Size = new System.Drawing.Size(26, 29);
            this.lblBestFitness.TabIndex = 15;
            this.lblBestFitness.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(326, 254);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 20);
            this.label9.TabIndex = 14;
            this.label9.Text = "Menor Fitness";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(7, 194);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(227, 42);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblGenerations
            // 
            this.lblGenerations.AutoSize = true;
            this.lblGenerations.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenerations.Location = new System.Drawing.Point(252, 247);
            this.lblGenerations.Name = "lblGenerations";
            this.lblGenerations.Size = new System.Drawing.Size(26, 29);
            this.lblGenerations.TabIndex = 12;
            this.lblGenerations.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(173, 254);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "Gerações";
            // 
            // txtMaxGenerations
            // 
            this.txtMaxGenerations.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxGenerations.Location = new System.Drawing.Point(174, 145);
            this.txtMaxGenerations.Name = "txtMaxGenerations";
            this.txtMaxGenerations.Size = new System.Drawing.Size(140, 26);
            this.txtMaxGenerations.TabIndex = 10;
            this.txtMaxGenerations.Text = "2000";
            this.txtMaxGenerations.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(197, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Max. Gerações";
            // 
            // lblStopWatch
            // 
            this.lblStopWatch.AutoSize = true;
            this.lblStopWatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopWatch.Location = new System.Drawing.Point(95, 247);
            this.lblStopWatch.Name = "lblStopWatch";
            this.lblStopWatch.Size = new System.Drawing.Size(58, 29);
            this.lblStopWatch.TabIndex = 8;
            this.lblStopWatch.Text = "0:00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Cronômetro";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(252, 194);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(227, 42);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Buscar";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtMutationRate
            // 
            this.txtMutationRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMutationRate.Location = new System.Drawing.Point(339, 145);
            this.txtMutationRate.Name = "txtMutationRate";
            this.txtMutationRate.Size = new System.Drawing.Size(140, 26);
            this.txtMutationRate.TabIndex = 5;
            this.txtMutationRate.Text = "0,20";
            this.txtMutationRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(356, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Taxa de Mutação";
            // 
            // txtPopulationSize
            // 
            this.txtPopulationSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPopulationSize.Location = new System.Drawing.Point(7, 145);
            this.txtPopulationSize.Name = "txtPopulationSize";
            this.txtPopulationSize.Size = new System.Drawing.Size(140, 26);
            this.txtPopulationSize.TabIndex = 3;
            this.txtPopulationSize.Text = "100";
            this.txtPopulationSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tamanho da População";
            // 
            // txtNomenclature
            // 
            this.txtNomenclature.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomenclature.Location = new System.Drawing.Point(6, 53);
            this.txtNomenclature.Multiline = true;
            this.txtNomenclature.Name = "txtNomenclature";
            this.txtNomenclature.Size = new System.Drawing.Size(473, 45);
            this.txtNomenclature.TabIndex = 1;
            this.txtNomenclature.Text = "H2O";
            this.txtNomenclature.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(157, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fórmula Molecular";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridResult);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(503, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 516);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Estrutura Molecular";
            // 
            // gridResult
            // 
            this.gridResult.AllowUserToAddRows = false;
            this.gridResult.AllowUserToDeleteRows = false;
            this.gridResult.AllowUserToOrderColumns = true;
            this.gridResult.AllowUserToResizeColumns = false;
            this.gridResult.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridResult.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridResult.BackgroundColor = System.Drawing.Color.White;
            this.gridResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.From,
            this.To});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridResult.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridResult.Location = new System.Drawing.Point(6, 22);
            this.gridResult.Name = "gridResult";
            this.gridResult.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridResult.RowHeadersVisible = false;
            this.gridResult.RowHeadersWidth = 50;
            this.gridResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResult.Size = new System.Drawing.Size(218, 488);
            this.gridResult.TabIndex = 0;
            // 
            // From
            // 
            this.From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.From.DataPropertyName = "From";
            this.From.HeaderText = "Átomo início";
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // To
            // 
            this.To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.To.DataPropertyName = "To";
            this.To.HeaderText = "Átomo destino";
            this.To.Name = "To";
            this.To.ReadOnly = true;
            this.To.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // chart1
            // 
            chartArea1.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.Title = "Geração";
            chartArea1.AxisX2.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX2.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.Title = "Fitness";
            chartArea1.AxisY2.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Fitness progression";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 338);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chart1.Size = new System.Drawing.Size(485, 190);
            this.chart1.TabIndex = 10;
            this.chart1.Text = "chart1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(30, 309);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "Status: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(91, 309);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 20);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Pronto";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MoleculeValidationForm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(737, 552);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MoleculeValidationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerador de Moléculas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MoleculeValidationForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNomenclature;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMutationRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPopulationSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStopWatch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridView gridResult;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMaxGenerations;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblGenerations;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblBestFitness;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
    }
}

