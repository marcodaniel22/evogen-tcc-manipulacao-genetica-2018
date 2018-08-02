using EvoGen.Domain.Interfaces.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EvoGen.MoleculeSearch
{
    public partial class MoleculeSearchForm : Form
    {
        private readonly IMoleculeService MleculeService;
        private bool Buscando;
        private bool Salvando;

        public MoleculeSearchForm(IMoleculeService moleculeService)
        {
            this.MleculeService = moleculeService;
            this.Buscando = false;
            this.Salvando = false;
            InitializeComponent();
        }

        private void MoleculeSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtQuantidade.Text = MleculeService.MoleculeCount().ToString();
            }
            catch(Exception)
            {
                txtQuantidade.Text = "Erro de conexão";
            }
        }

        private void btnIniciarBusca_Click(object sender, EventArgs e)
        {
            if (!Buscando)
                IniciarBusca();
            else
                PararBusca();
        }

        private void btnSalvarBanco_Click(object sender, EventArgs e)
        {
            if (!Salvando)
                SalvarEmBanco();
            else
                PararDeSalvarEmBanco();
        }

        private void IniciarBusca()
        {
            btnBusca.Text = "Parar Busca";
            btnBusca.BackColor = Color.Gray;
            Buscando = true;
        }

        private void PararBusca()
        {
            btnBusca.Text = "Iniciar Busca";
            btnBusca.BackColor = Color.DarkGray;
            Buscando = false;
        }

        private void SalvarEmBanco()
        {
            btnSalvar.Text = "Salvar de Salvar";
            btnSalvar.BackColor = Color.Gray;
            Salvando = true;
        }

        private void PararDeSalvarEmBanco()
        {
            btnSalvar.Text = "Salvar em Banco";
            btnSalvar.BackColor = Color.DarkGray;
            Salvando = false;
        }
    }
}
