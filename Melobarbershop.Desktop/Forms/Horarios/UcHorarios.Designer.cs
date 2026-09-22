namespace Melobarbershop.Desktop.Forms.Horarios;

partial class UcHorarios
{
    private System.ComponentModel.IContainer components = null;

    // ── Cabeçalho ──
    private Label lblTitulo;
    private Label lblSubtitulo;

    // ── Seção Grade Semanal ──
    private Label lblSecaoSemanal;
    private Panel pnlSemanal;
    private DataGridView dgvSemanal;
    private Button btnSalvarSemanal;
    private Label lblStatusSemanal;

    // ── Seção Datas Especiais ──
    private Label lblSecaoEspeciais;
    private Panel pnlEspeciais;
    private DataGridView dgvEspeciais;
    private Panel pnlAdicionarEspecial;

    // Campos para nova data especial
    private Label lblDataEspecial;
    private DateTimePicker dtpDataEspecial;
    private Label lblAbertoEspecial;
    private CheckBox chkAbertoEspecial;
    private Label lblInicioEspecial;
    private TextBox txtInicioEspecial;
    private Label lblFimEspecial;
    private TextBox txtFimEspecial;
    private Label lblDescricaoEspecial;
    private TextBox txtDescricaoEspecial;
    private Button btnAdicionarEspecial;
    private Button btnRemoverEspecial;
    private Label lblStatusEspeciais;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        lblSubtitulo = new Label();

        lblSecaoSemanal = new Label();
        pnlSemanal = new Panel();
        dgvSemanal = new DataGridView();
        btnSalvarSemanal = new Button();
        lblStatusSemanal = new Label();

        lblSecaoEspeciais = new Label();
        pnlEspeciais = new Panel();
        dgvEspeciais = new DataGridView();
        pnlAdicionarEspecial = new Panel();

        lblDataEspecial = new Label();
        dtpDataEspecial = new DateTimePicker();
        lblAbertoEspecial = new Label();
        chkAbertoEspecial = new CheckBox();
        lblInicioEspecial = new Label();
        txtInicioEspecial = new TextBox();
        lblFimEspecial = new Label();
        txtFimEspecial = new TextBox();
        lblDescricaoEspecial = new Label();
        txtDescricaoEspecial = new TextBox();
        btnAdicionarEspecial = new Button();
        btnRemoverEspecial = new Button();
        lblStatusEspeciais = new Label();

        pnlSemanal.SuspendLayout();
        pnlEspeciais.SuspendLayout();
        pnlAdicionarEspecial.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSemanal).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvEspeciais).BeginInit();
        SuspendLayout();

        // ── Título ──
        lblTitulo.Location = new Point(30, 20);
        lblTitulo.Size = new Size(700, 34);
        lblTitulo.Text = "🕐 Horários de Funcionamento";

        lblSubtitulo.Location = new Point(30, 56);
        lblSubtitulo.Size = new Size(700, 22);
        lblSubtitulo.Text = "Configure os horários de abertura e fechamento da barbearia por dia da semana e datas especiais.";

        // ── Seção Semanal ──
        lblSecaoSemanal.Location = new Point(30, 94);
        lblSecaoSemanal.Size = new Size(400, 22);
        lblSecaoSemanal.Text = "GRADE SEMANAL";

        // pnlSemanal
        pnlSemanal.Location = new Point(30, 118);
        pnlSemanal.Size = new Size(780, 260);
        pnlSemanal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // dgvSemanal
        dgvSemanal.Location = new Point(0, 0);
        dgvSemanal.Size = new Size(780, 210);
        dgvSemanal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvSemanal.ReadOnly = false;
        dgvSemanal.AllowUserToAddRows = false;
        dgvSemanal.AllowUserToDeleteRows = false;
        dgvSemanal.EditMode = DataGridViewEditMode.EditOnEnter;
        dgvSemanal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSemanal.CellValidating += dgvSemanal_CellValidating;

        btnSalvarSemanal.Location = new Point(0, 218);
        btnSalvarSemanal.Size = new Size(180, 36);
        btnSalvarSemanal.Text = "💾 Salvar Alterações";
        btnSalvarSemanal.Click += btnSalvarSemanal_Click;

        lblStatusSemanal.Location = new Point(190, 225);
        lblStatusSemanal.Size = new Size(580, 22);

        pnlSemanal.Controls.Add(dgvSemanal);
        pnlSemanal.Controls.Add(btnSalvarSemanal);
        pnlSemanal.Controls.Add(lblStatusSemanal);

        // ── Seção Especiais ──
        lblSecaoEspeciais.Location = new Point(30, 398);
        lblSecaoEspeciais.Size = new Size(400, 22);
        lblSecaoEspeciais.Text = "DATAS ESPECIAIS (FERIADOS / HORÁRIOS DIFERENCIADOS)";
        lblSecaoEspeciais.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        // pnlEspeciais
        pnlEspeciais.Location = new Point(30, 422);
        pnlEspeciais.Size = new Size(780, 280);
        pnlEspeciais.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // dgvEspeciais
        dgvEspeciais.Location = new Point(0, 0);
        dgvEspeciais.Size = new Size(780, 180);
        dgvEspeciais.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvEspeciais.ReadOnly = true;
        dgvEspeciais.AllowUserToAddRows = false;
        dgvEspeciais.AllowUserToDeleteRows = false;
        dgvEspeciais.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // pnlAdicionarEspecial
        pnlAdicionarEspecial.Location = new Point(0, 188);
        pnlAdicionarEspecial.Size = new Size(780, 88);
        pnlAdicionarEspecial.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // Data
        lblDataEspecial.Location = new Point(0, 6);
        lblDataEspecial.Size = new Size(40, 22);
        lblDataEspecial.Text = "Data:";

        dtpDataEspecial.Location = new Point(40, 4);
        dtpDataEspecial.Size = new Size(120, 26);
        dtpDataEspecial.Format = DateTimePickerFormat.Short;

        // Aberto
        lblAbertoEspecial.Location = new Point(170, 6);
        lblAbertoEspecial.Size = new Size(48, 22);
        lblAbertoEspecial.Text = "Aberto:";

        chkAbertoEspecial.Location = new Point(220, 6);
        chkAbertoEspecial.Size = new Size(24, 24);
        chkAbertoEspecial.Checked = true;
        chkAbertoEspecial.CheckedChanged += chkAbertoEspecial_CheckedChanged;

        // Início
        lblInicioEspecial.Location = new Point(254, 6);
        lblInicioEspecial.Size = new Size(44, 22);
        lblInicioEspecial.Text = "Início:";

        txtInicioEspecial.Location = new Point(300, 4);
        txtInicioEspecial.Size = new Size(65, 26);
        txtInicioEspecial.Text = "08:00";
        txtInicioEspecial.MaxLength = 5;

        // Fim
        lblFimEspecial.Location = new Point(374, 6);
        lblFimEspecial.Size = new Size(34, 22);
        lblFimEspecial.Text = "Fim:";

        txtFimEspecial.Location = new Point(410, 4);
        txtFimEspecial.Size = new Size(65, 26);
        txtFimEspecial.Text = "19:00";
        txtFimEspecial.MaxLength = 5;

        // Descrição
        lblDescricaoEspecial.Location = new Point(0, 40);
        lblDescricaoEspecial.Size = new Size(70, 22);
        lblDescricaoEspecial.Text = "Descrição:";

        txtDescricaoEspecial.Location = new Point(72, 38);
        txtDescricaoEspecial.Size = new Size(400, 26);
        txtDescricaoEspecial.MaxLength = 120;

        // Botões
        btnAdicionarEspecial.Location = new Point(480, 38);
        btnAdicionarEspecial.Size = new Size(150, 32);
        btnAdicionarEspecial.Text = "➕ Adicionar Data";
        btnAdicionarEspecial.Click += btnAdicionarEspecial_Click;

        btnRemoverEspecial.Location = new Point(638, 38);
        btnRemoverEspecial.Size = new Size(140, 32);
        btnRemoverEspecial.Text = "🗑 Remover Data";
        btnRemoverEspecial.Click += btnRemoverEspecial_Click;

        pnlAdicionarEspecial.Controls.Add(lblDataEspecial);
        pnlAdicionarEspecial.Controls.Add(dtpDataEspecial);
        pnlAdicionarEspecial.Controls.Add(lblAbertoEspecial);
        pnlAdicionarEspecial.Controls.Add(chkAbertoEspecial);
        pnlAdicionarEspecial.Controls.Add(lblInicioEspecial);
        pnlAdicionarEspecial.Controls.Add(txtInicioEspecial);
        pnlAdicionarEspecial.Controls.Add(lblFimEspecial);
        pnlAdicionarEspecial.Controls.Add(txtFimEspecial);
        pnlAdicionarEspecial.Controls.Add(lblDescricaoEspecial);
        pnlAdicionarEspecial.Controls.Add(txtDescricaoEspecial);
        pnlAdicionarEspecial.Controls.Add(btnAdicionarEspecial);
        pnlAdicionarEspecial.Controls.Add(btnRemoverEspecial);

        lblStatusEspeciais.Location = new Point(0, 278);
        lblStatusEspeciais.Size = new Size(780, 22);
        lblStatusEspeciais.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        pnlEspeciais.Controls.Add(dgvEspeciais);
        pnlEspeciais.Controls.Add(pnlAdicionarEspecial);
        pnlEspeciais.Controls.Add(lblStatusEspeciais);

        // ── UcHorarios ──
        this.Dock = DockStyle.Fill;
        this.Controls.Add(lblTitulo);
        this.Controls.Add(lblSubtitulo);
        this.Controls.Add(lblSecaoSemanal);
        this.Controls.Add(pnlSemanal);
        this.Controls.Add(lblSecaoEspeciais);
        this.Controls.Add(pnlEspeciais);

        pnlSemanal.ResumeLayout(false);
        pnlEspeciais.ResumeLayout(false);
        pnlAdicionarEspecial.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSemanal).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvEspeciais).EndInit();
        ResumeLayout(false);
    }
}
