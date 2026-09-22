namespace Melobarbershop.Desktop.Forms.Horarios;

partial class UcHorarios
{
    private System.ComponentModel.IContainer components = null;

    // ── Cabeçalho ──
    private Label lblTitulo;
    private Label lblSubtitulo;

    // ── Barra de Abas Modernas (Guna2) ──
    private Panel pnlBarraAbas;
    private Guna.UI2.WinForms.Guna2Button btnTabGeral;
    private Guna.UI2.WinForms.Guna2Button btnTabBloqueios;

    // ── Painel de Conteúdo Principal ──
    private Panel pnlConteudoPrincipal;

    // ── VIEW 1: Funcionamento Padrão & Especiais ──
    private Panel pnlViewGeral;
    private Label lblSecaoSemanal;
    private Panel pnlSemanalCard;
    private Guna.UI2.WinForms.Guna2DataGridView dgvSemanal;
    private Guna.UI2.WinForms.Guna2Button btnSalvarSemanal;
    private Label lblStatusSemanal;

    private Label lblSecaoEspeciais;
    private Panel pnlEspeciaisCard;
    private Guna.UI2.WinForms.Guna2DataGridView dgvEspeciais;
    private Panel pnlAdicionarEspecial;
    private Label lblDataEspecial;
    private Guna.UI2.WinForms.Guna2DateTimePicker dtpDataEspecial;
    private CheckBox chkAbertoEspecial;
    private Label lblInicioEspecial;
    private Guna.UI2.WinForms.Guna2TextBox txtInicioEspecial;
    private Label lblFimEspecial;
    private Guna.UI2.WinForms.Guna2TextBox txtFimEspecial;
    private Label lblDescricaoEspecial;
    private Guna.UI2.WinForms.Guna2TextBox txtDescricaoEspecial;
    private Guna.UI2.WinForms.Guna2Button btnAdicionarEspecial;
    private Guna.UI2.WinForms.Guna2Button btnRemoverEspecial;
    private Label lblStatusEspeciais;

    // ── VIEW 2: Bloqueios de Horário & Pausas ──
    private Panel pnlViewBloqueios;
    private TableLayoutPanel tlpBloqueios;
    private Panel pnlFiltroBloqueiosBarra;
    private Label lblFiltroBarbeiroBloqueio;
    private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroBarbeiroBloqueio;
    private Guna.UI2.WinForms.Guna2Button btnRecarregarBloqueios;
    private Guna.UI2.WinForms.Guna2Button btnNovoBloqueioAbreForm;
    private Guna.UI2.WinForms.Guna2Button btnRemoverBloqueio;

    private Panel pnlBloqueiosGridCard;
    private Guna.UI2.WinForms.Guna2DataGridView dgvBloqueios;

    // Card de formulário de novo bloqueio
    private Panel pnlCardNovoBloqueio;
    private Label lblTituloNovoBloqueio;
    private Label lblBloqueioBarbeiro;
    private Guna.UI2.WinForms.Guna2ComboBox cmbBloqueioBarbeiro;
    private Label lblBloqueioData;
    private Guna.UI2.WinForms.Guna2DateTimePicker dtpBloqueioData;
    private Label lblBloqueioHoraInicio;
    private Guna.UI2.WinForms.Guna2TextBox txtBloqueioHoraInicio;
    private Label lblBloqueioHoraFim;
    private Guna.UI2.WinForms.Guna2TextBox txtBloqueioHoraFim;
    private Label lblBloqueioMotivo;
    private Guna.UI2.WinForms.Guna2TextBox txtBloqueioMotivo;
    private Guna.UI2.WinForms.Guna2Button btnAdicionarBloqueio;
    private Label lblStatusBloqueios;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        lblSubtitulo = new Label();

        pnlBarraAbas = new Panel();
        btnTabGeral = new Guna.UI2.WinForms.Guna2Button();
        btnTabBloqueios = new Guna.UI2.WinForms.Guna2Button();

        pnlConteudoPrincipal = new Panel();

        // View 1
        pnlViewGeral = new Panel();
        lblSecaoSemanal = new Label();
        pnlSemanalCard = new Panel();
        dgvSemanal = new Guna.UI2.WinForms.Guna2DataGridView();
        btnSalvarSemanal = new Guna.UI2.WinForms.Guna2Button();
        lblStatusSemanal = new Label();

        lblSecaoEspeciais = new Label();
        pnlEspeciaisCard = new Panel();
        dgvEspeciais = new Guna.UI2.WinForms.Guna2DataGridView();
        pnlAdicionarEspecial = new Panel();
        lblDataEspecial = new Label();
        dtpDataEspecial = new Guna.UI2.WinForms.Guna2DateTimePicker();
        chkAbertoEspecial = new CheckBox();
        lblInicioEspecial = new Label();
        txtInicioEspecial = new Guna.UI2.WinForms.Guna2TextBox();
        lblFimEspecial = new Label();
        txtFimEspecial = new Guna.UI2.WinForms.Guna2TextBox();
        lblDescricaoEspecial = new Label();
        txtDescricaoEspecial = new Guna.UI2.WinForms.Guna2TextBox();
        btnAdicionarEspecial = new Guna.UI2.WinForms.Guna2Button();
        btnRemoverEspecial = new Guna.UI2.WinForms.Guna2Button();
        lblStatusEspeciais = new Label();

        // View 2
        pnlViewBloqueios = new Panel();
        tlpBloqueios = new TableLayoutPanel();
        pnlFiltroBloqueiosBarra = new Panel();
        lblFiltroBarbeiroBloqueio = new Label();
        cmbFiltroBarbeiroBloqueio = new Guna.UI2.WinForms.Guna2ComboBox();
        btnRecarregarBloqueios = new Guna.UI2.WinForms.Guna2Button();
        btnNovoBloqueioAbreForm = new Guna.UI2.WinForms.Guna2Button();
        btnRemoverBloqueio = new Guna.UI2.WinForms.Guna2Button();

        pnlBloqueiosGridCard = new Panel();
        dgvBloqueios = new Guna.UI2.WinForms.Guna2DataGridView();

        pnlCardNovoBloqueio = new Panel();
        lblTituloNovoBloqueio = new Label();
        lblBloqueioBarbeiro = new Label();
        cmbBloqueioBarbeiro = new Guna.UI2.WinForms.Guna2ComboBox();
        lblBloqueioData = new Label();
        dtpBloqueioData = new Guna.UI2.WinForms.Guna2DateTimePicker();
        lblBloqueioHoraInicio = new Label();
        txtBloqueioHoraInicio = new Guna.UI2.WinForms.Guna2TextBox();
        lblBloqueioHoraFim = new Label();
        txtBloqueioHoraFim = new Guna.UI2.WinForms.Guna2TextBox();
        lblBloqueioMotivo = new Label();
        txtBloqueioMotivo = new Guna.UI2.WinForms.Guna2TextBox();
        btnAdicionarBloqueio = new Guna.UI2.WinForms.Guna2Button();
        lblStatusBloqueios = new Label();

        pnlBarraAbas.SuspendLayout();
        pnlConteudoPrincipal.SuspendLayout();
        pnlViewGeral.SuspendLayout();
        pnlSemanalCard.SuspendLayout();
        pnlEspeciaisCard.SuspendLayout();
        pnlAdicionarEspecial.SuspendLayout();
        pnlViewBloqueios.SuspendLayout();
        tlpBloqueios.SuspendLayout();
        pnlFiltroBloqueiosBarra.SuspendLayout();
        pnlBloqueiosGridCard.SuspendLayout();
        pnlCardNovoBloqueio.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSemanal).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvEspeciais).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvBloqueios).BeginInit();
        SuspendLayout();

        // ── Título Superior ──
        lblTitulo.Location = new Point(25, 18);
        lblTitulo.Size = new Size(500, 30);
        lblTitulo.Text = "🕐 Gerenciamento de Horários";

        lblSubtitulo.Location = new Point(25, 48);
        lblSubtitulo.Size = new Size(700, 22);
        lblSubtitulo.Text = "Configure o expediente padrão da barbearia, feriados e bloqueios específicos de horários da equipe.";

        // ── Barra de Abas Modernas ──
        pnlBarraAbas.Location = new Point(25, 78);
        pnlBarraAbas.Size = new Size(950, 42);
        pnlBarraAbas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        btnTabGeral.Location = new Point(0, 0);
        btnTabGeral.Size = new Size(240, 40);
        btnTabGeral.Text = "📅  Grade Semanal & Feriados";
        btnTabGeral.Click += (s, e) => AlternarAba(0);

        btnTabBloqueios.Location = new Point(250, 0);
        btnTabBloqueios.Size = new Size(240, 40);
        btnTabBloqueios.Text = "⛔  Bloqueios & Pausas";
        btnTabBloqueios.Click += (s, e) => AlternarAba(1);

        pnlBarraAbas.Controls.Add(btnTabGeral);
        pnlBarraAbas.Controls.Add(btnTabBloqueios);

        // ── Painel de Conteúdo Principal ──
        pnlConteudoPrincipal.Location = new Point(25, 128);
        pnlConteudoPrincipal.Size = new Size(950, 540);
        pnlConteudoPrincipal.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // ═══════════════════════════════════════════════════════════════════
        // VIEW 1: GRADE SEMANAL E DATAS ESPECIAIS
        // ═══════════════════════════════════════════════════════════════════
        pnlViewGeral.Dock = DockStyle.Fill;
        pnlViewGeral.AutoScroll = true;

        // Seção Semanal
        lblSecaoSemanal.Location = new Point(0, 0);
        lblSecaoSemanal.Size = new Size(400, 22);
        lblSecaoSemanal.Text = "GRADE SEMANAL PADRÃO (REPETE TODAS AS SEMANAS)";

        pnlSemanalCard.Location = new Point(0, 26);
        pnlSemanalCard.Size = new Size(930, 220);
        pnlSemanalCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        dgvSemanal.Location = new Point(10, 10);
        dgvSemanal.Size = new Size(910, 160);
        dgvSemanal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvSemanal.ReadOnly = false;
        dgvSemanal.AllowUserToAddRows = false;
        dgvSemanal.AllowUserToDeleteRows = false;
        dgvSemanal.EditMode = DataGridViewEditMode.EditOnEnter;
        dgvSemanal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSemanal.CellValidating += dgvSemanal_CellValidating;

        btnSalvarSemanal.Location = new Point(10, 176);
        btnSalvarSemanal.Size = new Size(180, 36);
        btnSalvarSemanal.Text = "💾 Salvar Grade Semanal";
        btnSalvarSemanal.Click += btnSalvarSemanal_Click;

        lblStatusSemanal.Location = new Point(200, 182);
        lblStatusSemanal.Size = new Size(710, 22);

        pnlSemanalCard.Controls.Add(dgvSemanal);
        pnlSemanalCard.Controls.Add(btnSalvarSemanal);
        pnlSemanalCard.Controls.Add(lblStatusSemanal);

        // Seção Especiais
        lblSecaoEspeciais.Location = new Point(0, 256);
        lblSecaoEspeciais.Size = new Size(500, 22);
        lblSecaoEspeciais.Text = "DATAS ESPECIAIS / FERIADOS";

        pnlEspeciaisCard.Location = new Point(0, 282);
        pnlEspeciaisCard.Size = new Size(930, 280);
        pnlEspeciaisCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        dgvEspeciais.Location = new Point(10, 10);
        dgvEspeciais.Size = new Size(910, 150);
        dgvEspeciais.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvEspeciais.ReadOnly = true;
        dgvEspeciais.AllowUserToAddRows = false;
        dgvEspeciais.AllowUserToDeleteRows = false;
        dgvEspeciais.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        pnlAdicionarEspecial.Location = new Point(10, 168);
        pnlAdicionarEspecial.Size = new Size(910, 100);
        pnlAdicionarEspecial.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        lblDataEspecial.Location = new Point(0, 8);
        lblDataEspecial.Size = new Size(40, 22);
        lblDataEspecial.Text = "Data:";

        dtpDataEspecial.Location = new Point(45, 3);
        dtpDataEspecial.Size = new Size(130, 32);
        dtpDataEspecial.Format = DateTimePickerFormat.Short;

        chkAbertoEspecial.Location = new Point(190, 7);
        chkAbertoEspecial.Size = new Size(75, 24);
        chkAbertoEspecial.Text = "Aberto";
        chkAbertoEspecial.Checked = true;
        chkAbertoEspecial.CheckedChanged += chkAbertoEspecial_CheckedChanged;

        lblInicioEspecial.Location = new Point(275, 8);
        lblInicioEspecial.Size = new Size(45, 22);
        lblInicioEspecial.Text = "Início:";

        txtInicioEspecial.Location = new Point(325, 3);
        txtInicioEspecial.Size = new Size(70, 32);
        txtInicioEspecial.Text = "08:00";
        txtInicioEspecial.MaxLength = 5;

        lblFimEspecial.Location = new Point(405, 8);
        lblFimEspecial.Size = new Size(35, 22);
        lblFimEspecial.Text = "Fim:";

        txtFimEspecial.Location = new Point(445, 3);
        txtFimEspecial.Size = new Size(70, 32);
        txtFimEspecial.Text = "19:00";
        txtFimEspecial.MaxLength = 5;

        lblDescricaoEspecial.Location = new Point(0, 48);
        lblDescricaoEspecial.Size = new Size(70, 22);
        lblDescricaoEspecial.Text = "Descrição:";

        txtDescricaoEspecial.Location = new Point(75, 43);
        txtDescricaoEspecial.Size = new Size(440, 32);
        txtDescricaoEspecial.MaxLength = 120;
        txtDescricaoEspecial.PlaceholderText = "Ex: Feriado de Tiradentes";

        btnAdicionarEspecial.Location = new Point(530, 43);
        btnAdicionarEspecial.Size = new Size(130, 32);
        btnAdicionarEspecial.Text = "➕ Adicionar";
        btnAdicionarEspecial.Click += btnAdicionarEspecial_Click;

        btnRemoverEspecial.Location = new Point(670, 43);
        btnRemoverEspecial.Size = new Size(130, 32);
        btnRemoverEspecial.Text = "🗑 Remover";
        btnRemoverEspecial.Click += btnRemoverEspecial_Click;

        lblStatusEspeciais.Location = new Point(0, 78);
        lblStatusEspeciais.Size = new Size(800, 20);

        pnlAdicionarEspecial.Controls.Add(lblDataEspecial);
        pnlAdicionarEspecial.Controls.Add(dtpDataEspecial);
        pnlAdicionarEspecial.Controls.Add(chkAbertoEspecial);
        pnlAdicionarEspecial.Controls.Add(lblInicioEspecial);
        pnlAdicionarEspecial.Controls.Add(txtInicioEspecial);
        pnlAdicionarEspecial.Controls.Add(lblFimEspecial);
        pnlAdicionarEspecial.Controls.Add(txtFimEspecial);
        pnlAdicionarEspecial.Controls.Add(lblDescricaoEspecial);
        pnlAdicionarEspecial.Controls.Add(txtDescricaoEspecial);
        pnlAdicionarEspecial.Controls.Add(btnAdicionarEspecial);
        pnlAdicionarEspecial.Controls.Add(btnRemoverEspecial);
        pnlAdicionarEspecial.Controls.Add(lblStatusEspeciais);

        pnlEspeciaisCard.Controls.Add(dgvEspeciais);
        pnlEspeciaisCard.Controls.Add(pnlAdicionarEspecial);

        pnlViewGeral.Controls.Add(lblSecaoSemanal);
        pnlViewGeral.Controls.Add(pnlSemanalCard);
        pnlViewGeral.Controls.Add(lblSecaoEspeciais);
        pnlViewGeral.Controls.Add(pnlEspeciaisCard);

        // ═══════════════════════════════════════════════════════════════════
        // VIEW 2: BLOQUEIOS DE HORÁRIO & PAUSAS
        // Usa TableLayoutPanel para evitar sobreposição e garantir redimensionamento correto
        // ═══════════════════════════════════════════════════════════════════
        pnlViewBloqueios.Dock = DockStyle.Fill;

        // TableLayoutPanel: Linha 0 = Filtros (auto), Linha 1 = Grid (stretch), Linha 2 = Formulário (auto)
        tlpBloqueios.Dock = DockStyle.Fill;
        tlpBloqueios.ColumnCount = 1;
        tlpBloqueios.RowCount = 3;
        tlpBloqueios.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpBloqueios.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));    // Filtros
        tlpBloqueios.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // Grid (expansível)
        tlpBloqueios.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));   // Formulário

        // Barra de Ações e Filtro de Bloqueios
        pnlFiltroBloqueiosBarra.Dock = DockStyle.Fill;
        pnlFiltroBloqueiosBarra.Padding = new Padding(0, 6, 0, 6);

        lblFiltroBarbeiroBloqueio.Location = new Point(0, 10);
        lblFiltroBarbeiroBloqueio.Size = new Size(110, 24);
        lblFiltroBarbeiroBloqueio.Text = "Filtrar Barbeiro:";

        cmbFiltroBarbeiroBloqueio.Location = new Point(115, 4);
        cmbFiltroBarbeiroBloqueio.Size = new Size(220, 36);
        cmbFiltroBarbeiroBloqueio.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFiltroBarbeiroBloqueio.SelectedIndexChanged += cmbFiltroBarbeiroBloqueio_SelectedIndexChanged;

        btnRecarregarBloqueios.Location = new Point(345, 4);
        btnRecarregarBloqueios.Size = new Size(120, 36);
        btnRecarregarBloqueios.Text = "🔄 Atualizar";
        btnRecarregarBloqueios.Click += btnRecarregarBloqueios_Click;

        btnNovoBloqueioAbreForm.Location = new Point(475, 4);
        btnNovoBloqueioAbreForm.Size = new Size(165, 36);
        btnNovoBloqueioAbreForm.Text = "➕ Novo Bloqueio";
        btnNovoBloqueioAbreForm.Click += btnNovoBloqueioAbreForm_Click;

        btnRemoverBloqueio.Location = new Point(650, 4);
        btnRemoverBloqueio.Size = new Size(175, 36);
        btnRemoverBloqueio.Text = "🔓 Desbloquear Horário";
        btnRemoverBloqueio.Click += btnRemoverBloqueio_Click;

        pnlFiltroBloqueiosBarra.Controls.Add(lblFiltroBarbeiroBloqueio);
        pnlFiltroBloqueiosBarra.Controls.Add(cmbFiltroBarbeiroBloqueio);
        pnlFiltroBloqueiosBarra.Controls.Add(btnRecarregarBloqueios);
        pnlFiltroBloqueiosBarra.Controls.Add(btnNovoBloqueioAbreForm);
        pnlFiltroBloqueiosBarra.Controls.Add(btnRemoverBloqueio);

        // Painel do Grid — ocupa toda a célula da linha 1
        pnlBloqueiosGridCard.Dock = DockStyle.Fill;
        pnlBloqueiosGridCard.Padding = new Padding(0, 6, 0, 6);

        dgvBloqueios.Dock = DockStyle.Fill;
        dgvBloqueios.Margin = new Padding(0);
        dgvBloqueios.ReadOnly = true;
        dgvBloqueios.AllowUserToAddRows = false;
        dgvBloqueios.AllowUserToDeleteRows = false;
        dgvBloqueios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        pnlBloqueiosGridCard.Controls.Add(dgvBloqueios);

        // Painel Card de Adicionar Bloqueio — linha 2 (altura fixa 160px)
        pnlCardNovoBloqueio.Dock = DockStyle.Fill;
        pnlCardNovoBloqueio.Padding = new Padding(12, 8, 12, 8);

        lblTituloNovoBloqueio.Location = new Point(12, 8);
        lblTituloNovoBloqueio.Size = new Size(500, 22);
        lblTituloNovoBloqueio.Text = "CADASTRAR BLOQUEIO DE HORÁRIO / PAUSA";

        // Linha 1 do form: Barbeiro | Data | Hora Início | Hora Fim
        lblBloqueioBarbeiro.Location = new Point(12, 40);
        lblBloqueioBarbeiro.Size = new Size(65, 22);
        lblBloqueioBarbeiro.Text = "Barbeiro:";

        cmbBloqueioBarbeiro.Location = new Point(80, 34);
        cmbBloqueioBarbeiro.Size = new Size(200, 34);
        cmbBloqueioBarbeiro.DropDownStyle = ComboBoxStyle.DropDownList;

        lblBloqueioData.Location = new Point(290, 40);
        lblBloqueioData.Size = new Size(40, 22);
        lblBloqueioData.Text = "Data:";

        dtpBloqueioData.Location = new Point(333, 34);
        dtpBloqueioData.Size = new Size(130, 34);
        dtpBloqueioData.Format = DateTimePickerFormat.Short;

        lblBloqueioHoraInicio.Location = new Point(473, 40);
        lblBloqueioHoraInicio.Size = new Size(75, 22);
        lblBloqueioHoraInicio.Text = "Hora Início:";

        txtBloqueioHoraInicio.Location = new Point(550, 34);
        txtBloqueioHoraInicio.Size = new Size(70, 34);
        txtBloqueioHoraInicio.Text = "12:00";
        txtBloqueioHoraInicio.MaxLength = 5;
        txtBloqueioHoraInicio.PlaceholderText = "12:00";

        lblBloqueioHoraFim.Location = new Point(630, 40);
        lblBloqueioHoraFim.Size = new Size(65, 22);
        lblBloqueioHoraFim.Text = "Hora Fim:";

        txtBloqueioHoraFim.Location = new Point(698, 34);
        txtBloqueioHoraFim.Size = new Size(70, 34);
        txtBloqueioHoraFim.Text = "13:00";
        txtBloqueioHoraFim.MaxLength = 5;
        txtBloqueioHoraFim.PlaceholderText = "13:00";

        // Linha 2 do form: Motivo | Botão Confirmar | Status
        lblBloqueioMotivo.Location = new Point(12, 84);
        lblBloqueioMotivo.Size = new Size(65, 22);
        lblBloqueioMotivo.Text = "Motivo:";

        txtBloqueioMotivo.Location = new Point(80, 78);
        txtBloqueioMotivo.Size = new Size(400, 34);
        txtBloqueioMotivo.PlaceholderText = "Ex: Intervalo / Almoço";
        txtBloqueioMotivo.MaxLength = 100;

        btnAdicionarBloqueio.Location = new Point(490, 78);
        btnAdicionarBloqueio.Size = new Size(190, 34);
        btnAdicionarBloqueio.Text = "⛔ Confirmar Bloqueio";
        btnAdicionarBloqueio.Click += btnAdicionarBloqueio_Click;

        lblStatusBloqueios.Location = new Point(690, 84);
        lblStatusBloqueios.Size = new Size(230, 24);
        lblStatusBloqueios.AutoEllipsis = true;

        pnlCardNovoBloqueio.Controls.Add(lblTituloNovoBloqueio);
        pnlCardNovoBloqueio.Controls.Add(lblBloqueioBarbeiro);
        pnlCardNovoBloqueio.Controls.Add(cmbBloqueioBarbeiro);
        pnlCardNovoBloqueio.Controls.Add(lblBloqueioData);
        pnlCardNovoBloqueio.Controls.Add(dtpBloqueioData);
        pnlCardNovoBloqueio.Controls.Add(lblBloqueioHoraInicio);
        pnlCardNovoBloqueio.Controls.Add(txtBloqueioHoraInicio);
        pnlCardNovoBloqueio.Controls.Add(lblBloqueioHoraFim);
        pnlCardNovoBloqueio.Controls.Add(txtBloqueioHoraFim);
        pnlCardNovoBloqueio.Controls.Add(lblBloqueioMotivo);
        pnlCardNovoBloqueio.Controls.Add(txtBloqueioMotivo);
        pnlCardNovoBloqueio.Controls.Add(btnAdicionarBloqueio);
        pnlCardNovoBloqueio.Controls.Add(lblStatusBloqueios);

        // Monta o TableLayoutPanel
        tlpBloqueios.Controls.Add(pnlFiltroBloqueiosBarra, 0, 0);
        tlpBloqueios.Controls.Add(pnlBloqueiosGridCard, 0, 1);
        tlpBloqueios.Controls.Add(pnlCardNovoBloqueio, 0, 2);

        pnlViewBloqueios.Controls.Add(tlpBloqueios);

        // Adiciona views no container principal
        pnlConteudoPrincipal.Controls.Add(pnlViewGeral);
        pnlConteudoPrincipal.Controls.Add(pnlViewBloqueios);

        // ── UcHorarios ──
        this.Dock = DockStyle.Fill;
        this.Size = new Size(1000, 680);
        this.Controls.Add(lblTitulo);
        this.Controls.Add(lblSubtitulo);
        this.Controls.Add(pnlBarraAbas);
        this.Controls.Add(pnlConteudoPrincipal);

        pnlBarraAbas.ResumeLayout(false);
        pnlConteudoPrincipal.ResumeLayout(false);
        pnlSemanalCard.ResumeLayout(false);
        pnlEspeciaisCard.ResumeLayout(false);
        pnlAdicionarEspecial.ResumeLayout(false);
        pnlViewGeral.ResumeLayout(false);
        tlpBloqueios.ResumeLayout(false);
        pnlFiltroBloqueiosBarra.ResumeLayout(false);
        pnlBloqueiosGridCard.ResumeLayout(false);
        pnlCardNovoBloqueio.ResumeLayout(false);
        pnlViewBloqueios.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSemanal).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvEspeciais).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvBloqueios).EndInit();
        ResumeLayout(false);
    }
}


