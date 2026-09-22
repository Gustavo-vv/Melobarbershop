namespace Melobarbershop.Desktop.Forms.Usuarios;

partial class FormEditarCliente
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblSubtitulo;

    private Label lblNome;
    private TextBox txtNome;

    private Label lblTelefone;
    private TextBox txtTelefone;

    private Label lblDataNascimento;
    private DateTimePicker dtpDataNascimento;
    private CheckBox chkSemDataNascimento;

    private Label lblPreferencias;
    private TextBox txtPreferencias;

    private Label lblComissao;
    private NumericUpDown nudComissao;
    private Label lblComissaoSufixo;
    private Panel pnlComissao;

    private CheckBox chkAtivo;

    private Label lblStatus;
    private Button btnSalvar;
    private Button btnCancelar;

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

        lblNome = new Label();
        txtNome = new TextBox();

        lblTelefone = new Label();
        txtTelefone = new TextBox();

        lblDataNascimento = new Label();
        dtpDataNascimento = new DateTimePicker();
        chkSemDataNascimento = new CheckBox();

        lblPreferencias = new Label();
        txtPreferencias = new TextBox();

        pnlComissao = new Panel();
        lblComissao = new Label();
        nudComissao = new NumericUpDown();
        lblComissaoSufixo = new Label();

        chkAtivo = new CheckBox();
        lblStatus = new Label();
        btnSalvar = new Button();
        btnCancelar = new Button();

        ((System.ComponentModel.ISupportInitialize)nudComissao).BeginInit();
        SuspendLayout();

        // lblTitulo
        lblTitulo.Location = new Point(24, 20);
        lblTitulo.Size = new Size(500, 32);
        lblTitulo.Text = "Editar Usuário";

        // lblSubtitulo
        lblSubtitulo.Location = new Point(24, 54);
        lblSubtitulo.Size = new Size(500, 20);
        lblSubtitulo.Text = "Altere os dados cadastrais do usuário selecionado.";

        // ----- Nome -----
        lblNome.Location = new Point(24, 90);
        lblNome.Size = new Size(200, 20);
        lblNome.Text = "Nome completo *";

        txtNome.Location = new Point(24, 112);
        txtNome.Size = new Size(480, 28);
        txtNome.MaxLength = 150;

        // ----- Telefone -----
        lblTelefone.Location = new Point(24, 152);
        lblTelefone.Size = new Size(200, 20);
        lblTelefone.Text = "Telefone / WhatsApp";

        txtTelefone.Location = new Point(24, 174);
        txtTelefone.Size = new Size(240, 28);
        txtTelefone.MaxLength = 20;

        // ----- Data Nascimento -----
        lblDataNascimento.Location = new Point(24, 214);
        lblDataNascimento.Size = new Size(200, 20);
        lblDataNascimento.Text = "Data de Nascimento";

        dtpDataNascimento.Location = new Point(24, 236);
        dtpDataNascimento.Size = new Size(200, 28);
        dtpDataNascimento.Format = DateTimePickerFormat.Short;
        dtpDataNascimento.MaxDate = DateTime.Today;

        chkSemDataNascimento.Location = new Point(236, 239);
        chkSemDataNascimento.Size = new Size(160, 20);
        chkSemDataNascimento.Text = "Não informar";
        chkSemDataNascimento.CheckedChanged += chkSemDataNascimento_CheckedChanged;

        // ----- Preferências -----
        lblPreferencias.Location = new Point(24, 276);
        lblPreferencias.Size = new Size(200, 20);
        lblPreferencias.Text = "Preferências / Observações";

        txtPreferencias.Location = new Point(24, 298);
        txtPreferencias.Size = new Size(480, 70);
        txtPreferencias.Multiline = true;
        txtPreferencias.MaxLength = 500;
        txtPreferencias.ScrollBars = ScrollBars.Vertical;

        // ----- Comissão (somente Barbeiro) -----
        pnlComissao.Location = new Point(24, 380);
        pnlComissao.Size = new Size(300, 50);
        pnlComissao.Visible = false;

        lblComissao.Location = new Point(0, 0);
        lblComissao.Size = new Size(200, 20);
        lblComissao.Text = "Percentual de Comissão (%)";

        nudComissao.Location = new Point(0, 22);
        nudComissao.Size = new Size(90, 26);
        nudComissao.Minimum = 0;
        nudComissao.Maximum = 100;
        nudComissao.DecimalPlaces = 2;

        lblComissaoSufixo.Location = new Point(98, 26);
        lblComissaoSufixo.Size = new Size(20, 20);
        lblComissaoSufixo.Text = "%";

        pnlComissao.Controls.Add(lblComissao);
        pnlComissao.Controls.Add(nudComissao);
        pnlComissao.Controls.Add(lblComissaoSufixo);

        // ----- Status ativo -----
        chkAtivo.Location = new Point(24, 440);
        chkAtivo.Size = new Size(200, 24);
        chkAtivo.Text = "Conta ativa";
        chkAtivo.Checked = true;

        // ----- Status label -----
        lblStatus.Location = new Point(24, 474);
        lblStatus.Size = new Size(480, 22);

        // ----- Botões -----
        btnSalvar.Location = new Point(316, 508);
        btnSalvar.Size = new Size(110, 36);
        btnSalvar.Text = "Salvar";
        btnSalvar.Click += btnSalvar_Click;

        btnCancelar.Location = new Point(188, 508);
        btnCancelar.Size = new Size(110, 36);
        btnCancelar.Text = "Cancelar";
        btnCancelar.Click += btnCancelar_Click;

        // ----- Form -----
        ClientSize = new Size(528, 564);
        Controls.Add(lblTitulo);
        Controls.Add(lblSubtitulo);
        Controls.Add(lblNome);
        Controls.Add(txtNome);
        Controls.Add(lblTelefone);
        Controls.Add(txtTelefone);
        Controls.Add(lblDataNascimento);
        Controls.Add(dtpDataNascimento);
        Controls.Add(chkSemDataNascimento);
        Controls.Add(lblPreferencias);
        Controls.Add(txtPreferencias);
        Controls.Add(pnlComissao);
        Controls.Add(chkAtivo);
        Controls.Add(lblStatus);
        Controls.Add(btnSalvar);
        Controls.Add(btnCancelar);

        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;

        ((System.ComponentModel.ISupportInitialize)nudComissao).EndInit();
        ResumeLayout(false);
    }
}
