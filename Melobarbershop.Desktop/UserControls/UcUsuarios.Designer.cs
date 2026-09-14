namespace Melobarbershop.Desktop.UserControls
{
    partial class UcUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;

        private Panel panelBarraAcoes;
        private Button btnAlternarStatus;
        private Button btnAtualizar;

        private Label lblFiltro;
        private ComboBox cmbFiltroRole;

        private Label lblBusca;
        private TextBox txtBusca;
        private Label lblStatus;

        private DataGridView dgvUsuarios;

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
            btnAlternarStatus = new Button();
            btnAtualizar = new Button();

            lblFiltro = new Label();
            cmbFiltroRole = new ComboBox();

            lblBusca = new Label();
            txtBusca = new TextBox();
            lblStatus = new Label();

            dgvUsuarios = new DataGridView();

            panelBarraAcoes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(400, 32);
            lblTitulo.Text = "Gestão de Usuários e Equipe";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(25, 52);
            lblSubtitulo.Size = new Size(450, 24);
            lblSubtitulo.Text = "Consulta e controle de status de barbeiros, clientes e administradores";

            // panelBarraAcoes
            panelBarraAcoes.Location = new Point(25, 90);
            panelBarraAcoes.Size = new Size(935, 45);
            panelBarraAcoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelBarraAcoes.Controls.Add(btnAlternarStatus);
            panelBarraAcoes.Controls.Add(lblFiltro);
            panelBarraAcoes.Controls.Add(cmbFiltroRole);
            panelBarraAcoes.Controls.Add(btnAtualizar);

            // btnAlternarStatus
            btnAlternarStatus.Location = new Point(0, 3);
            btnAlternarStatus.Size = new Size(180, 36);
            btnAlternarStatus.Text = "⚡ Ativar/Desativar Usuário";
            btnAlternarStatus.Click += btnAlternarStatus_Click;

            // lblFiltro
            lblFiltro.Location = new Point(200, 10);
            lblFiltro.Size = new Size(90, 22);
            lblFiltro.Text = "Filtrar perfil:";
            lblFiltro.ForeColor = Color.FromArgb(160, 163, 175);

            // cmbFiltroRole
            cmbFiltroRole.Location = new Point(295, 7);
            cmbFiltroRole.Size = new Size(180, 28);
            cmbFiltroRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroRole.SelectedIndexChanged += cmbFiltroRole_SelectedIndexChanged;

            // btnAtualizar
            btnAtualizar.Location = new Point(815, 3);
            btnAtualizar.Size = new Size(120, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "🔄 Recarregar";
            btnAtualizar.Click += btnAtualizar_Click;

            // lblBusca
            lblBusca.Location = new Point(25, 148);
            lblBusca.Size = new Size(120, 22);
            lblBusca.Text = "Buscar por nome:";
            lblBusca.ForeColor = Color.FromArgb(160, 163, 175);

            // txtBusca
            txtBusca.Location = new Point(155, 145);
            txtBusca.Size = new Size(280, 28);
            txtBusca.BackColor = Color.FromArgb(40, 42, 52);
            txtBusca.ForeColor = Color.White;
            txtBusca.BorderStyle = BorderStyle.FixedSingle;
            txtBusca.TextChanged += txtBusca_TextChanged;

            // lblStatus
            lblStatus.Location = new Point(450, 148);
            lblStatus.Size = new Size(510, 22);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // dgvUsuarios
            dgvUsuarios.Location = new Point(25, 185);
            dgvUsuarios.Size = new Size(935, 425);
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;

            // UserControl
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(panelBarraAcoes);
            Controls.Add(lblBusca);
            Controls.Add(txtBusca);
            Controls.Add(lblStatus);
            Controls.Add(dgvUsuarios);

            Size = new Size(980, 630);

            panelBarraAcoes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
