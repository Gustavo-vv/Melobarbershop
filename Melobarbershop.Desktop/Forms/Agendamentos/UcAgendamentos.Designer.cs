namespace Melobarbershop.Desktop.Forms.Agendamentos
{
    partial class UcAgendamentos
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;

        private Panel panelBarraAcoes;
        private Label lblFiltroPeriodo;
        private ComboBox cmbFiltroPeriodo;
        private Panel pnlDatasPersonalizadas;
        private Label lblAte;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFim;
        private Button btnBuscar;
        private Button btnAtualizar;

        private Button btnConfirmar;
        private Button btnConcluir;
        private Button btnCancelar;

        private Label lblBusca;
        private TextBox txtBusca;
        private Label lblStatus;

        private DataGridView dgvAgendamentos;

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
            lblFiltroPeriodo = new Label();
            cmbFiltroPeriodo = new ComboBox();
            pnlDatasPersonalizadas = new Panel();
            dtpInicio = new DateTimePicker();
            lblAte = new Label();
            dtpFim = new DateTimePicker();
            btnBuscar = new Button();
            btnAtualizar = new Button();

            btnConfirmar = new Button();
            btnConcluir = new Button();
            btnCancelar = new Button();

            lblBusca = new Label();
            txtBusca = new TextBox();
            lblStatus = new Label();

            dgvAgendamentos = new DataGridView();

            panelBarraAcoes.SuspendLayout();
            pnlDatasPersonalizadas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(400, 32);
            lblTitulo.Text = "Gestão de Agendamentos";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(25, 52);
            lblSubtitulo.Size = new Size(500, 24);
            lblSubtitulo.Text = "Acompanhamento da agenda da barbearia, status e atendimento dos clientes";

            // panelBarraAcoes
            panelBarraAcoes.Location = new Point(25, 90);
            panelBarraAcoes.Size = new Size(935, 45);
            panelBarraAcoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblFiltroPeriodo
            lblFiltroPeriodo.Location = new Point(0, 10);
            lblFiltroPeriodo.Size = new Size(60, 22);
            lblFiltroPeriodo.Text = "Período:";
            lblFiltroPeriodo.ForeColor = Color.FromArgb(160, 163, 175);

            // cmbFiltroPeriodo
            cmbFiltroPeriodo.Location = new Point(65, 7);
            cmbFiltroPeriodo.Size = new Size(130, 28);
            cmbFiltroPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroPeriodo.SelectedIndexChanged += cmbFiltroPeriodo_SelectedIndexChanged;

            // pnlDatasPersonalizadas
            pnlDatasPersonalizadas.Location = new Point(200, 2);
            pnlDatasPersonalizadas.Size = new Size(240, 40);
            pnlDatasPersonalizadas.Visible = false;
            pnlDatasPersonalizadas.Controls.Add(dtpInicio);
            pnlDatasPersonalizadas.Controls.Add(lblAte);
            pnlDatasPersonalizadas.Controls.Add(dtpFim);

            dtpInicio.Location = new Point(0, 5);
            dtpInicio.Size = new Size(105, 28);

            lblAte.Location = new Point(108, 8);
            lblAte.Size = new Size(24, 20);
            lblAte.Text = "a";
            lblAte.TextAlign = ContentAlignment.MiddleCenter;
            lblAte.ForeColor = Color.FromArgb(160, 163, 175);

            dtpFim.Location = new Point(135, 5);
            dtpFim.Size = new Size(105, 28);

            // btnBuscar
            btnBuscar.Location = new Point(445, 4);
            btnBuscar.Size = new Size(90, 36);
            btnBuscar.Text = "🔍 Filtrar";
            btnBuscar.Click += btnBuscar_Click;

            // btnConfirmar
            btnConfirmar.Location = new Point(545, 4);
            btnConfirmar.Size = new Size(110, 36);
            btnConfirmar.Text = "✔️ Confirmar";
            btnConfirmar.Click += btnConfirmar_Click;

            // btnConcluir
            btnConcluir.Location = new Point(660, 4);
            btnConcluir.Size = new Size(100, 36);
            btnConcluir.Text = "🏁 Concluir";
            btnConcluir.Click += btnConcluir_Click;

            // btnCancelar
            btnCancelar.Location = new Point(765, 4);
            btnCancelar.Size = new Size(95, 36);
            btnCancelar.Text = "❌ Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            // btnAtualizar
            btnAtualizar.Location = new Point(865, 4);
            btnAtualizar.Size = new Size(70, 36);
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Text = "🔄";
            btnAtualizar.Click += btnAtualizar_Click;

            panelBarraAcoes.Controls.Add(lblFiltroPeriodo);
            panelBarraAcoes.Controls.Add(cmbFiltroPeriodo);
            panelBarraAcoes.Controls.Add(pnlDatasPersonalizadas);
            panelBarraAcoes.Controls.Add(btnBuscar);
            panelBarraAcoes.Controls.Add(btnConfirmar);
            panelBarraAcoes.Controls.Add(btnConcluir);
            panelBarraAcoes.Controls.Add(btnCancelar);
            panelBarraAcoes.Controls.Add(btnAtualizar);

            // lblBusca
            lblBusca.Location = new Point(25, 148);
            lblBusca.Size = new Size(110, 22);
            lblBusca.Text = "Buscar agenda:";
            lblBusca.ForeColor = Color.FromArgb(160, 163, 175);

            // txtBusca
            txtBusca.Location = new Point(140, 145);
            txtBusca.Size = new Size(290, 28);
            txtBusca.BackColor = Color.FromArgb(40, 42, 52);
            txtBusca.ForeColor = Color.White;
            txtBusca.BorderStyle = BorderStyle.FixedSingle;
            txtBusca.TextChanged += txtBusca_TextChanged;

            // lblStatus
            lblStatus.Location = new Point(440, 148);
            lblStatus.Size = new Size(520, 22);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // dgvAgendamentos
            dgvAgendamentos.Location = new Point(25, 185);
            dgvAgendamentos.Size = new Size(935, 425);
            dgvAgendamentos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAgendamentos.ReadOnly = true;
            dgvAgendamentos.AllowUserToAddRows = false;
            dgvAgendamentos.AllowUserToDeleteRows = false;
            dgvAgendamentos.DataBindingComplete += dgvAgendamentos_DataBindingComplete;

            // UserControl
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(panelBarraAcoes);
            Controls.Add(lblBusca);
            Controls.Add(txtBusca);
            Controls.Add(lblStatus);
            Controls.Add(dgvAgendamentos);

            Size = new Size(980, 630);

            panelBarraAcoes.ResumeLayout(false);
            pnlDatasPersonalizadas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAgendamentos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
