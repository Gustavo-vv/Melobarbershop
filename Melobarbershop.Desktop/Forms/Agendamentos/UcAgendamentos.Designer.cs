namespace Melobarbershop.Desktop.Forms.Agendamentos
{
    partial class UcAgendamentos
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnAtualizar;

        private Panel pnlFiltrosTopo;
        private Label lblFiltroPeriodo;
        private ComboBox cmbFiltroPeriodo;
        private Panel pnlDatasPersonalizadas;
        private Label lblAte;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFim;

        private Panel pnlStatusChips;
        private FlowLayoutPanel flowStatusChips;

        private Panel pnlBuscaContainer;
        private Label lblBuscaIcon;
        private TextBox txtBusca;
        private Label lblStatus;

        private Panel pnlGridContainer;
        private DataGridView dgvAgendamentos;
        private ContextMenuStrip cmsAcoes;
        private ToolStripMenuItem tsmiCancelar;
        private ToolStripMenuItem tsmiNaoCompareceu;

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
            btnAtualizar = new Button();

            pnlFiltrosTopo = new Panel();
            lblFiltroPeriodo = new Label();
            cmbFiltroPeriodo = new ComboBox();
            pnlDatasPersonalizadas = new Panel();
            dtpInicio = new DateTimePicker();
            lblAte = new Label();
            dtpFim = new DateTimePicker();

            pnlStatusChips = new Panel();
            flowStatusChips = new FlowLayoutPanel();

            pnlBuscaContainer = new Panel();
            lblBuscaIcon = new Label();
            txtBusca = new TextBox();
            lblStatus = new Label();

            pnlGridContainer = new Panel();
            dgvAgendamentos = new DataGridView();
            cmsAcoes = new ContextMenuStrip();
            tsmiCancelar = new ToolStripMenuItem();
            tsmiNaoCompareceu = new ToolStripMenuItem();

            pnlFiltrosTopo.SuspendLayout();
            pnlDatasPersonalizadas.SuspendLayout();
            pnlBuscaContainer.SuspendLayout();
            pnlGridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).BeginInit();
            cmsAcoes.SuspendLayout();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Size = new Size(420, 32);
            lblTitulo.Text = "Gestão de Agendamentos";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(24, 52);
            lblSubtitulo.Size = new Size(520, 24);
            lblSubtitulo.Text = "Acompanhamento da agenda da barbearia, status e atendimento dos clientes";

            // btnAtualizar
            btnAtualizar.Location = new Point(810, 20);
            btnAtualizar.Size = new Size(145, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "🔄 Recarregar";
            btnAtualizar.Click += btnAtualizar_Click;

            // pnlFiltrosTopo
            pnlFiltrosTopo.Location = new Point(24, 86);
            pnlFiltrosTopo.Size = new Size(932, 42);
            pnlFiltrosTopo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltrosTopo.Controls.Add(lblFiltroPeriodo);
            pnlFiltrosTopo.Controls.Add(cmbFiltroPeriodo);
            pnlFiltrosTopo.Controls.Add(pnlDatasPersonalizadas);

            // lblFiltroPeriodo
            lblFiltroPeriodo.Location = new Point(0, 8);
            lblFiltroPeriodo.Size = new Size(65, 24);
            lblFiltroPeriodo.Text = "Período:";

            // cmbFiltroPeriodo
            cmbFiltroPeriodo.Location = new Point(70, 5);
            cmbFiltroPeriodo.Size = new Size(170, 28);
            cmbFiltroPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroPeriodo.SelectedIndexChanged += cmbFiltroPeriodo_SelectedIndexChanged;

            // pnlDatasPersonalizadas
            pnlDatasPersonalizadas.Location = new Point(250, 0);
            pnlDatasPersonalizadas.Size = new Size(270, 38);
            pnlDatasPersonalizadas.Visible = false;
            pnlDatasPersonalizadas.Controls.Add(dtpInicio);
            pnlDatasPersonalizadas.Controls.Add(lblAte);
            pnlDatasPersonalizadas.Controls.Add(dtpFim);

            dtpInicio.Location = new Point(0, 5);
            dtpInicio.Size = new Size(115, 28);
            dtpInicio.ValueChanged += dtpPersonalizado_ValueChanged;

            lblAte.Location = new Point(120, 8);
            lblAte.Size = new Size(20, 22);
            lblAte.Text = "a";
            lblAte.TextAlign = ContentAlignment.MiddleCenter;

            dtpFim.Location = new Point(145, 5);
            dtpFim.Size = new Size(115, 28);
            dtpFim.ValueChanged += dtpPersonalizado_ValueChanged;

            // pnlStatusChips
            pnlStatusChips.Location = new Point(24, 130);
            pnlStatusChips.Size = new Size(932, 38);
            pnlStatusChips.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatusChips.Controls.Add(flowStatusChips);

            // flowStatusChips
            flowStatusChips.Dock = DockStyle.Fill;
            flowStatusChips.WrapContents = false;
            flowStatusChips.AutoScroll = true;

            // pnlBuscaContainer
            pnlBuscaContainer.Location = new Point(24, 172);
            pnlBuscaContainer.Size = new Size(932, 36);
            pnlBuscaContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlBuscaContainer.Controls.Add(lblBuscaIcon);
            pnlBuscaContainer.Controls.Add(txtBusca);
            pnlBuscaContainer.Controls.Add(lblStatus);

            // lblBuscaIcon
            lblBuscaIcon.Location = new Point(0, 5);
            lblBuscaIcon.Size = new Size(28, 24);
            lblBuscaIcon.Text = "🔍";
            lblBuscaIcon.TextAlign = ContentAlignment.MiddleCenter;

            // txtBusca
            txtBusca.Location = new Point(32, 4);
            txtBusca.Size = new Size(340, 28);
            txtBusca.TextChanged += txtBusca_TextChanged;

            // lblStatus
            lblStatus.Location = new Point(390, 6);
            lblStatus.Size = new Size(540, 22);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // pnlGridContainer
            pnlGridContainer.Location = new Point(24, 214);
            pnlGridContainer.Size = new Size(932, 396);
            pnlGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGridContainer.Controls.Add(dgvAgendamentos);

            // dgvAgendamentos
            dgvAgendamentos.Dock = DockStyle.Fill;
            dgvAgendamentos.ReadOnly = true;
            dgvAgendamentos.AllowUserToAddRows = false;
            dgvAgendamentos.AllowUserToDeleteRows = false;
            dgvAgendamentos.ContextMenuStrip = cmsAcoes;
            dgvAgendamentos.CellContentClick += dgvAgendamentos_CellContentClick;
            dgvAgendamentos.CellPainting += dgvAgendamentos_CellPainting;
            dgvAgendamentos.CellMouseDown += dgvAgendamentos_CellMouseDown;

            // cmsAcoes
            cmsAcoes.Items.Add(tsmiCancelar);
            cmsAcoes.Items.Add(tsmiNaoCompareceu);

            // tsmiCancelar
            tsmiCancelar.Text = "❌ Cancelar este Agendamento";
            tsmiCancelar.Click += tsmiCancelar_Click;

            // tsmiNaoCompareceu
            tsmiNaoCompareceu.Text = "👤 Não compareceu";
            tsmiNaoCompareceu.Click += tsmiNaoCompareceu_Click;

            // UserControl
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnAtualizar);
            Controls.Add(pnlFiltrosTopo);
            Controls.Add(pnlStatusChips);
            Controls.Add(pnlBuscaContainer);
            Controls.Add(pnlGridContainer);

            Size = new Size(980, 630);

            pnlFiltrosTopo.ResumeLayout(false);
            pnlDatasPersonalizadas.ResumeLayout(false);
            pnlBuscaContainer.ResumeLayout(false);
            pnlBuscaContainer.PerformLayout();
            pnlGridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).EndInit();
            cmsAcoes.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
