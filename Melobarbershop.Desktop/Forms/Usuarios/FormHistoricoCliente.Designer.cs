namespace Melobarbershop.Desktop.Forms.Usuarios;

partial class FormHistoricoCliente
{
    private System.ComponentModel.IContainer components = null;

    // ---- Painel Esquerdo — Perfil ----
    private Panel pnlPerfil;
    private Panel pnlAvatar;
    private Label lblAvatarIniciais;
    private Label lblNomeCliente;
    private Label lblEmailCliente;
    private Label lblTelefoneCliente;
    private Label lblNascimentoLabel;
    private Label lblNascimentoCliente;
    private Label lblCadastroLabel;
    private Label lblCadastroCliente;
    private Label lblRolesLabel;
    private Label lblRolesCliente;
    private Panel pnlDivider1;
    private Panel pnlDivider2;

    // Preferências
    private Label lblPrefLabel;
    private Label lblPrefTexto;

    // ---- Painel Direito — Histórico ----
    private Panel pnlHistorico;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Button btnFechar;
    private Label lblStatus;
    private DataGridView dgvHistorico;

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
        // Painel esquerdo — Perfil
        pnlPerfil = new Panel();
        pnlAvatar = new Panel();
        lblAvatarIniciais = new Label();
        lblNomeCliente = new Label();
        lblEmailCliente = new Label();
        lblTelefoneCliente = new Label();
        lblNascimentoLabel = new Label();
        lblNascimentoCliente = new Label();
        lblCadastroLabel = new Label();
        lblCadastroCliente = new Label();
        lblRolesLabel = new Label();
        lblRolesCliente = new Label();
        pnlDivider1 = new Panel();
        pnlDivider2 = new Panel();

        lblPrefLabel = new Label();
        lblPrefTexto = new Label();

        // Painel direito — Histórico
        pnlHistorico = new Panel();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        btnFechar = new Button();
        lblStatus = new Label();
        dgvHistorico = new DataGridView();

        pnlPerfil.SuspendLayout();
        pnlHistorico.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorico).BeginInit();
        SuspendLayout();

        // ================================================================
        // pnlPerfil (esquerda)
        // ================================================================
        pnlPerfil.Location = new Point(0, 0);
        pnlPerfil.Size = new Size(270, 640);
        pnlPerfil.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlPerfil.Padding = new Padding(16, 20, 16, 16);

        // pnlAvatar (círculo com iniciais)
        pnlAvatar.Location = new Point(75, 24);
        pnlAvatar.Size = new Size(120, 120);
        pnlAvatar.Paint += pnlAvatar_Paint;

        lblAvatarIniciais.Location = new Point(0, 0);
        lblAvatarIniciais.Size = new Size(120, 120);
        lblAvatarIniciais.TextAlign = ContentAlignment.MiddleCenter;
        lblAvatarIniciais.Text = "AB";
        pnlAvatar.Controls.Add(lblAvatarIniciais);

        // Nome
        lblNomeCliente.Location = new Point(16, 158);
        lblNomeCliente.Size = new Size(238, 28);
        lblNomeCliente.TextAlign = ContentAlignment.MiddleCenter;
        lblNomeCliente.Text = "Nome do Cliente";

        // Email
        lblEmailCliente.Location = new Point(16, 186);
        lblEmailCliente.Size = new Size(238, 20);
        lblEmailCliente.TextAlign = ContentAlignment.MiddleCenter;
        lblEmailCliente.Text = "email@exemplo.com";

        // Telefone
        lblTelefoneCliente.Location = new Point(16, 206);
        lblTelefoneCliente.Size = new Size(238, 20);
        lblTelefoneCliente.TextAlign = ContentAlignment.MiddleCenter;
        lblTelefoneCliente.Text = "-";

        // Divider 1
        pnlDivider1.Location = new Point(16, 238);
        pnlDivider1.Size = new Size(238, 1);

        // Nascimento
        lblNascimentoLabel.Location = new Point(16, 248);
        lblNascimentoLabel.Size = new Size(100, 18);
        lblNascimentoLabel.Text = "Nascimento";

        lblNascimentoCliente.Location = new Point(116, 248);
        lblNascimentoCliente.Size = new Size(138, 18);
        lblNascimentoCliente.TextAlign = ContentAlignment.MiddleRight;
        lblNascimentoCliente.Text = "-";

        // Cadastro
        lblCadastroLabel.Location = new Point(16, 270);
        lblCadastroLabel.Size = new Size(100, 18);
        lblCadastroLabel.Text = "Membro desde";

        lblCadastroCliente.Location = new Point(116, 270);
        lblCadastroCliente.Size = new Size(138, 18);
        lblCadastroCliente.TextAlign = ContentAlignment.MiddleRight;
        lblCadastroCliente.Text = "-";

        // Roles
        lblRolesLabel.Location = new Point(16, 292);
        lblRolesLabel.Size = new Size(100, 18);
        lblRolesLabel.Text = "Perfil";

        lblRolesCliente.Location = new Point(116, 292);
        lblRolesCliente.Size = new Size(138, 18);
        lblRolesCliente.TextAlign = ContentAlignment.MiddleRight;
        lblRolesCliente.Text = "-";

        // Divider 2
        pnlDivider2.Location = new Point(16, 322);
        pnlDivider2.Size = new Size(238, 1);

        // Preferências
        lblPrefLabel.Location = new Point(16, 336);
        lblPrefLabel.Size = new Size(238, 18);
        lblPrefLabel.Text = "PREFERÊNCIAS";

        lblPrefTexto.Location = new Point(16, 360);
        lblPrefTexto.Size = new Size(238, 240);
        lblPrefTexto.Text = "-";

        pnlPerfil.Controls.Add(pnlAvatar);
        pnlPerfil.Controls.Add(lblNomeCliente);
        pnlPerfil.Controls.Add(lblEmailCliente);
        pnlPerfil.Controls.Add(lblTelefoneCliente);
        pnlPerfil.Controls.Add(pnlDivider1);
        pnlPerfil.Controls.Add(lblNascimentoLabel);
        pnlPerfil.Controls.Add(lblNascimentoCliente);
        pnlPerfil.Controls.Add(lblCadastroLabel);
        pnlPerfil.Controls.Add(lblCadastroCliente);
        pnlPerfil.Controls.Add(lblRolesLabel);
        pnlPerfil.Controls.Add(lblRolesCliente);
        pnlPerfil.Controls.Add(pnlDivider2);
        pnlPerfil.Controls.Add(lblPrefLabel);
        pnlPerfil.Controls.Add(lblPrefTexto);

        // ================================================================
        // pnlHistorico (direita)
        // ================================================================
        pnlHistorico.Location = new Point(270, 0);
        pnlHistorico.Size = new Size(790, 640);
        pnlHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // lblTitulo
        lblTitulo.Location = new Point(20, 20);
        lblTitulo.Size = new Size(560, 32);
        lblTitulo.Text = "Histórico de Atendimentos";

        // lblSubtitulo
        lblSubtitulo.Location = new Point(20, 54);
        lblSubtitulo.Size = new Size(560, 20);
        lblSubtitulo.Text = "Todos os agendamentos realizados pelo cliente (mais recentes primeiro)";

        // btnFechar
        btnFechar.Location = new Point(664, 22);
        btnFechar.Size = new Size(110, 36);
        btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFechar.Text = "Fechar";
        btnFechar.Click += btnFechar_Click;

        // lblStatus
        lblStatus.Location = new Point(20, 86);
        lblStatus.Size = new Size(754, 22);
        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // dgvHistorico
        dgvHistorico.Location = new Point(20, 114);
        dgvHistorico.Size = new Size(754, 502);
        dgvHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvHistorico.ReadOnly = true;
        dgvHistorico.AllowUserToAddRows = false;
        dgvHistorico.AllowUserToDeleteRows = false;
        dgvHistorico.CellPainting += dgvHistorico_CellPainting;

        pnlHistorico.Controls.Add(lblTitulo);
        pnlHistorico.Controls.Add(lblSubtitulo);
        pnlHistorico.Controls.Add(btnFechar);
        pnlHistorico.Controls.Add(lblStatus);
        pnlHistorico.Controls.Add(dgvHistorico);

        // ================================================================
        // FormHistoricoCliente
        // ================================================================
        ClientSize = new Size(1060, 640);
        Controls.Add(pnlPerfil);
        Controls.Add(pnlHistorico);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(900, 580);
        MaximizeBox = true;
        MinimizeBox = false;
        ShowInTaskbar = false;

        pnlPerfil.ResumeLayout(false);
        pnlHistorico.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHistorico).EndInit();
        ResumeLayout(false);
    }
}
