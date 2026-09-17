namespace Melobarbershop.Desktop.Forms.Agendamentos
{
    partial class UcAgendamentos
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;

        private Panel pnlFiltrosTopo;
        private Label lblFiltroPeriodo;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroPeriodo;
        private Panel pnlDatasPersonalizadas;
        private Label lblAte;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpInicio;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFim;

        private Guna.UI2.WinForms.Guna2TextBox txtBusca;
        private Label lblStatus;

        private Panel pnlStatusChips;
        private FlowLayoutPanel flowStatusChips;

        private Panel pnlGridContainer;
        private Guna.UI2.WinForms.Guna2DataGridView dgvAgendamentos;
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
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();

            pnlFiltrosTopo = new Panel();
            lblFiltroPeriodo = new Label();
            cmbFiltroPeriodo = new Guna.UI2.WinForms.Guna2ComboBox();
            pnlDatasPersonalizadas = new Panel();
            dtpInicio = new Guna.UI2.WinForms.Guna2DateTimePicker();
            lblAte = new Label();
            dtpFim = new Guna.UI2.WinForms.Guna2DateTimePicker();

            txtBusca = new Guna.UI2.WinForms.Guna2TextBox();
            lblStatus = new Label();

            pnlStatusChips = new Panel();
            flowStatusChips = new FlowLayoutPanel();

            pnlGridContainer = new Panel();
            dgvAgendamentos = new Guna.UI2.WinForms.Guna2DataGridView();
            cmsAcoes = new ContextMenuStrip();
            tsmiCancelar = new ToolStripMenuItem();
            tsmiNaoCompareceu = new ToolStripMenuItem();

            pnlFiltrosTopo.SuspendLayout();
            pnlDatasPersonalizadas.SuspendLayout();
            pnlStatusChips.SuspendLayout();
            pnlGridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).BeginInit();
            cmsAcoes.SuspendLayout();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(24, 18);
            lblTitulo.Size = new Size(420, 30);
            lblTitulo.Text = "Gestão de Agendamentos";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(24, 48);
            lblSubtitulo.Size = new Size(520, 22);
            lblSubtitulo.Text = "Acompanhamento da agenda da barbearia, status e atendimento dos clientes";

            // btnAtualizar
            btnAtualizar.Location = new Point(810, 18);
            btnAtualizar.Size = new Size(145, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "🔄 Recarregar";
            btnAtualizar.Click += btnAtualizar_Click;

            // pnlFiltrosTopo
            pnlFiltrosTopo.Location = new Point(24, 78);
            pnlFiltrosTopo.Size = new Size(932, 42);
            pnlFiltrosTopo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltrosTopo.Controls.Add(lblFiltroPeriodo);
            pnlFiltrosTopo.Controls.Add(cmbFiltroPeriodo);
            pnlFiltrosTopo.Controls.Add(pnlDatasPersonalizadas);
            pnlFiltrosTopo.Controls.Add(txtBusca);
            pnlFiltrosTopo.Controls.Add(lblStatus);

            // lblFiltroPeriodo
            lblFiltroPeriodo.Location = new Point(0, 8);
            lblFiltroPeriodo.Size = new Size(65, 26);
            lblFiltroPeriodo.Text = "Período:";

            // cmbFiltroPeriodo
            cmbFiltroPeriodo.Location = new Point(70, 3);
            cmbFiltroPeriodo.Size = new Size(160, 36);
            cmbFiltroPeriodo.SelectedIndexChanged += cmbFiltroPeriodo_SelectedIndexChanged;

            // pnlDatasPersonalizadas
            pnlDatasPersonalizadas.Location = new Point(235, 0);
            pnlDatasPersonalizadas.Size = new Size(270, 42);
            pnlDatasPersonalizadas.Visible = false;
            pnlDatasPersonalizadas.Controls.Add(dtpInicio);
            pnlDatasPersonalizadas.Controls.Add(lblAte);
            pnlDatasPersonalizadas.Controls.Add(dtpFim);

            dtpInicio.Location = new Point(0, 3);
            dtpInicio.Size = new Size(115, 36);
            dtpInicio.ValueChanged += dtpPersonalizado_ValueChanged;

            lblAte.Location = new Point(120, 8);
            lblAte.Size = new Size(20, 26);
            lblAte.Text = "a";
            lblAte.TextAlign = ContentAlignment.MiddleCenter;

            dtpFim.Location = new Point(145, 3);
            dtpFim.Size = new Size(115, 36);
            dtpFim.ValueChanged += dtpPersonalizado_ValueChanged;

            // txtBusca
            txtBusca.Location = new Point(515, 3);
            txtBusca.Size = new Size(260, 36);
            txtBusca.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtBusca.PlaceholderText = "🔍 Buscar cliente/barbeiro...";
            txtBusca.TextChanged += txtBusca_TextChanged;

            // lblStatus
            lblStatus.Location = new Point(780, 8);
            lblStatus.Size = new Size(150, 26);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // pnlStatusChips
            pnlStatusChips.Location = new Point(24, 126);
            pnlStatusChips.Size = new Size(932, 40);
            pnlStatusChips.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlStatusChips.Controls.Add(flowStatusChips);

            // flowStatusChips
            flowStatusChips.Dock = DockStyle.Fill;
            flowStatusChips.WrapContents = false;
            flowStatusChips.AutoScroll = true;

            // pnlGridContainer
            pnlGridContainer.Location = new Point(24, 172);
            pnlGridContainer.Size = new Size(932, 438);
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
            Controls.Add(pnlGridContainer);

            Size = new Size(980, 630);

            pnlFiltrosTopo.ResumeLayout(false);
            pnlDatasPersonalizadas.ResumeLayout(false);
            pnlStatusChips.ResumeLayout(false);
            pnlGridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).EndInit();
            cmsAcoes.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
