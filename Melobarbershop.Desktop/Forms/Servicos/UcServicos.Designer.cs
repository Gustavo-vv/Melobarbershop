namespace Melobarbershop.Desktop.Forms.Servicos
{
    partial class UcServicos
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;

        private Panel panelBarraAcoes;
        private Guna.UI2.WinForms.Guna2Button btnNovo;
        private Guna.UI2.WinForms.Guna2Button btnEditar;
        private Guna.UI2.WinForms.Guna2Button btnAlternarStatus;
        private Guna.UI2.WinForms.Guna2Button btnExcluir;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;

        private Label lblBusca;
        private Guna.UI2.WinForms.Guna2TextBox txtBusca;
        private Label lblStatus;

        private Guna.UI2.WinForms.Guna2DataGridView dgvServicos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblSubtitulo = new Label();

            panelBarraAcoes = new Panel();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnAlternarStatus = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();

            lblBusca = new Label();
            txtBusca = new Guna.UI2.WinForms.Guna2TextBox();
            lblStatus = new Label();

            dgvServicos = new Guna.UI2.WinForms.Guna2DataGridView();

            panelBarraAcoes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvServicos).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(400, 32);
            lblTitulo.Text = "Gestão de Serviços";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(25, 52);
            lblSubtitulo.Size = new Size(450, 24);
            lblSubtitulo.Text = "Cadastro, edição, precificação e visibilidade no catálogo";

            // panelBarraAcoes
            panelBarraAcoes.Location = new Point(25, 90);
            panelBarraAcoes.Size = new Size(935, 45);
            panelBarraAcoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelBarraAcoes.Controls.Add(btnNovo);
            panelBarraAcoes.Controls.Add(btnEditar);
            panelBarraAcoes.Controls.Add(btnAlternarStatus);
            panelBarraAcoes.Controls.Add(btnExcluir);
            panelBarraAcoes.Controls.Add(btnAtualizar);

            // btnNovo
            btnNovo.Size = new Size(130, 36);
            btnNovo.Text = "+ Novo Serviço";
            btnNovo.Click += btnNovo_Click;

            // btnEditar
            btnEditar.Size = new Size(100, 36);
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;

            // btnAlternarStatus
            btnAlternarStatus.Size = new Size(150, 36);
            btnAlternarStatus.Text = "Ativar / Desativar";
            btnAlternarStatus.Click += btnAlternarStatus_Click;

            // btnExcluir
            btnExcluir.Size = new Size(100, 36);
            btnExcluir.Text = "Excluir";
            btnExcluir.Click += btnExcluir_Click;

            // btnAtualizar — fixo à direita, não entra no cálculo de centralização
            btnAtualizar.Location = new Point(815, 3);
            btnAtualizar.Size = new Size(120, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "Recarregar";
            btnAtualizar.Click += btnAtualizar_Click;

            // Centralização dinâmica do bloco de botões de ação
            CentralizarBotoesBarra();
            panelBarraAcoes.Resize += (s, e) => CentralizarBotoesBarra();

            // lblBusca
            lblBusca.Location = new Point(25, 148);
            lblBusca.Size = new Size(120, 22);
            lblBusca.Text = "Buscar serviço:";
            lblBusca.ForeColor = Color.FromArgb(160, 163, 175);

            // txtBusca
            txtBusca.Location = new Point(135, 145);
            txtBusca.Size = new Size(300, 28);
            txtBusca.TextChanged += txtBusca_TextChanged;

            // lblStatus
            lblStatus.Location = new Point(450, 148);
            lblStatus.Size = new Size(510, 22);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // dgvServicos
            dgvServicos.Location = new Point(25, 185);
            dgvServicos.Size = new Size(935, 425);
            dgvServicos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvServicos.ReadOnly = true;
            dgvServicos.AllowUserToAddRows = false;
            dgvServicos.AllowUserToDeleteRows = false;

            // UserControl
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(panelBarraAcoes);
            Controls.Add(lblBusca);
            Controls.Add(txtBusca);
            Controls.Add(lblStatus);
            Controls.Add(dgvServicos);

            Size = new Size(980, 630);

            panelBarraAcoes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvServicos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
