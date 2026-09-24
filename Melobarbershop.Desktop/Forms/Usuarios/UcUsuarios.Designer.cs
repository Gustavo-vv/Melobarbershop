namespace Melobarbershop.Desktop.Forms.Usuarios
{
    partial class UcUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;

        private Panel panelBarraAcoes;
        private Guna.UI2.WinForms.Guna2Button btnAlternarStatus;
        private Guna.UI2.WinForms.Guna2Button btnHistoricoCliente;
        private Guna.UI2.WinForms.Guna2Button btnEditarCliente;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;

        private Label lblFiltro;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroRole;

        private Label lblBusca;
        private Guna.UI2.WinForms.Guna2TextBox txtBusca;
        private Label lblStatus;

        private Guna.UI2.WinForms.Guna2DataGridView dgvUsuarios;

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
            btnAlternarStatus = new Guna.UI2.WinForms.Guna2Button();
            btnHistoricoCliente = new Guna.UI2.WinForms.Guna2Button();
            btnEditarCliente = new Guna.UI2.WinForms.Guna2Button();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();

            lblFiltro = new Label();
            cmbFiltroRole = new Guna.UI2.WinForms.Guna2ComboBox();

            lblBusca = new Label();
            txtBusca = new Guna.UI2.WinForms.Guna2TextBox();
            lblStatus = new Label();

            dgvUsuarios = new Guna.UI2.WinForms.Guna2DataGridView();

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
            panelBarraAcoes.Controls.Add(btnHistoricoCliente);
            panelBarraAcoes.Controls.Add(btnEditarCliente);
            panelBarraAcoes.Controls.Add(lblFiltro);
            panelBarraAcoes.Controls.Add(cmbFiltroRole);
            panelBarraAcoes.Controls.Add(btnAtualizar);

            // btnAlternarStatus
            btnAlternarStatus.Location = new Point(0, 3);
            btnAlternarStatus.Size = new Size(150, 36);
            btnAlternarStatus.Text = "Ativar / Desativar";
            btnAlternarStatus.Click += btnAlternarStatus_Click;

            // btnHistoricoCliente
            btnHistoricoCliente.Location = new Point(158, 3);
            btnHistoricoCliente.Size = new Size(130, 36);
            btnHistoricoCliente.Text = "Ver Histórico";
            btnHistoricoCliente.Click += btnHistoricoCliente_Click;

            // btnEditarCliente
            btnEditarCliente.Location = new Point(296, 3);
            btnEditarCliente.Size = new Size(136, 36);
            btnEditarCliente.Text = "Editar Dados";
            btnEditarCliente.Click += btnEditarCliente_Click;

            // lblFiltro
            lblFiltro.Location = new Point(440, 10);
            lblFiltro.Size = new Size(85, 22);
            lblFiltro.Text = "Filtrar perfil:";
            lblFiltro.ForeColor = Color.FromArgb(160, 163, 175);

            // cmbFiltroRole
            cmbFiltroRole.Location = new Point(528, 7);
            cmbFiltroRole.Size = new Size(175, 28);
            cmbFiltroRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroRole.SelectedIndexChanged += cmbFiltroRole_SelectedIndexChanged;

            // btnAtualizar
            btnAtualizar.Location = new Point(815, 3);
            btnAtualizar.Size = new Size(120, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "Recarregar";
            btnAtualizar.Click += btnAtualizar_Click;

            // lblBusca
            lblBusca.Location = new Point(25, 148);
            lblBusca.Size = new Size(120, 22);
            lblBusca.Text = "Buscar por nome:";
            lblBusca.ForeColor = Color.FromArgb(160, 163, 175);

            // txtBusca
            txtBusca.Location = new Point(155, 145);
            txtBusca.Size = new Size(280, 28);
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
            dgvUsuarios.CellContentClick += dgvUsuarios_CellContentClick;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;

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
