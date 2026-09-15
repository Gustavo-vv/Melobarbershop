namespace Melobarbershop.Desktop.Forms.Dashboard
{
    partial class UcDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnAtualizar;
        private Label lblStatus;

        private Panel cardServicos;
        private Label lblCardServicosValor;
        private Label lblCardServicosTitulo;
        private Label lblCardServicosSub;

        private Panel cardServicosAtivos;
        private Label lblCardAtivosValor;
        private Label lblCardAtivosTitulo;
        private Label lblCardAtivosSub;

        private Panel cardClientes;
        private Label lblCardClientesValor;
        private Label lblCardClientesTitulo;
        private Label lblCardClientesSub;

        private Panel cardBarbeiros;
        private Label lblCardBarbeirosValor;
        private Label lblCardBarbeirosTitulo;
        private Label lblCardBarbeirosSub;

        private Panel panelAviso;
        private Label lblAvisoTitulo;
        private Label lblAvisoDesc;

        private Label lblSecaoResumo;
        private DataGridView dgvResumo;

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
            lblStatus = new Label();

            cardServicos = new Panel();
            lblCardServicosValor = new Label();
            lblCardServicosTitulo = new Label();
            lblCardServicosSub = new Label();

            cardServicosAtivos = new Panel();
            lblCardAtivosValor = new Label();
            lblCardAtivosTitulo = new Label();
            lblCardAtivosSub = new Label();

            cardClientes = new Panel();
            lblCardClientesValor = new Label();
            lblCardClientesTitulo = new Label();
            lblCardClientesSub = new Label();

            cardBarbeiros = new Panel();
            lblCardBarbeirosValor = new Label();
            lblCardBarbeirosTitulo = new Label();
            lblCardBarbeirosSub = new Label();

            panelAviso = new Panel();
            lblAvisoTitulo = new Label();
            lblAvisoDesc = new Label();

            lblSecaoResumo = new Label();
            dgvResumo = new DataGridView();

            cardServicos.SuspendLayout();
            cardServicosAtivos.SuspendLayout();
            cardClientes.SuspendLayout();
            cardBarbeiros.SuspendLayout();
            panelAviso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResumo).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(400, 32);
            lblTitulo.Text = "Visão Geral do Negócio";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(25, 52);
            lblSubtitulo.Size = new Size(450, 24);
            lblSubtitulo.Text = "Métricas em tempo real sincronizadas com a Melobarbershop.API";

            // btnAtualizar
            btnAtualizar.Location = new Point(780, 25);
            btnAtualizar.Size = new Size(160, 38);
            btnAtualizar.Text = "🔄 Atualizar Dados";
            btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAtualizar.Click += btnAtualizar_Click;

            // lblStatus
            lblStatus.Location = new Point(600, 68);
            lblStatus.Size = new Size(340, 20);
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // cardServicos
            cardServicos.Location = new Point(25, 95);
            cardServicos.Size = new Size(215, 110);
            cardServicos.Controls.Add(lblCardServicosValor);
            cardServicos.Controls.Add(lblCardServicosTitulo);
            cardServicos.Controls.Add(lblCardServicosSub);

            lblCardServicosValor.Location = new Point(15, 12);
            lblCardServicosValor.Size = new Size(185, 42);
            lblCardServicosValor.Text = "--";

            lblCardServicosTitulo.Location = new Point(15, 58);
            lblCardServicosTitulo.Size = new Size(185, 22);
            lblCardServicosTitulo.Text = "Total de Serviços";

            lblCardServicosSub.Location = new Point(15, 82);
            lblCardServicosSub.Size = new Size(185, 20);
            lblCardServicosSub.Text = "Carregando...";

            // cardServicosAtivos
            cardServicosAtivos.Location = new Point(265, 95);
            cardServicosAtivos.Size = new Size(215, 110);
            cardServicosAtivos.Controls.Add(lblCardAtivosValor);
            cardServicosAtivos.Controls.Add(lblCardAtivosTitulo);
            cardServicosAtivos.Controls.Add(lblCardAtivosSub);

            lblCardAtivosValor.Location = new Point(15, 12);
            lblCardAtivosValor.Size = new Size(185, 42);
            lblCardAtivosValor.Text = "--";

            lblCardAtivosTitulo.Location = new Point(15, 58);
            lblCardAtivosTitulo.Size = new Size(185, 22);
            lblCardAtivosTitulo.Text = "Serviços em Oferta";

            lblCardAtivosSub.Location = new Point(15, 82);
            lblCardAtivosSub.Size = new Size(185, 20);
            lblCardAtivosSub.Text = "Carregando...";

            // cardClientes
            cardClientes.Location = new Point(505, 95);
            cardClientes.Size = new Size(215, 110);
            cardClientes.Controls.Add(lblCardClientesValor);
            cardClientes.Controls.Add(lblCardClientesTitulo);
            cardClientes.Controls.Add(lblCardClientesSub);

            lblCardClientesValor.Location = new Point(15, 12);
            lblCardClientesValor.Size = new Size(185, 42);
            lblCardClientesValor.Text = "--";

            lblCardClientesTitulo.Location = new Point(15, 58);
            lblCardClientesTitulo.Size = new Size(185, 22);
            lblCardClientesTitulo.Text = "Clientes na Base";

            lblCardClientesSub.Location = new Point(15, 82);
            lblCardClientesSub.Size = new Size(185, 20);
            lblCardClientesSub.Text = "Carregando...";

            // cardBarbeiros
            cardBarbeiros.Location = new Point(745, 95);
            cardBarbeiros.Size = new Size(215, 110);
            cardBarbeiros.Controls.Add(lblCardBarbeirosValor);
            cardBarbeiros.Controls.Add(lblCardBarbeirosTitulo);
            cardBarbeiros.Controls.Add(lblCardBarbeirosSub);

            lblCardBarbeirosValor.Location = new Point(15, 12);
            lblCardBarbeirosValor.Size = new Size(185, 42);
            lblCardBarbeirosValor.Text = "--";

            lblCardBarbeirosTitulo.Location = new Point(15, 58);
            lblCardBarbeirosTitulo.Size = new Size(185, 22);
            lblCardBarbeirosTitulo.Text = "Barbeiros na Equipe";

            lblCardBarbeirosSub.Location = new Point(15, 82);
            lblCardBarbeirosSub.Size = new Size(185, 20);
            lblCardBarbeirosSub.Text = "Carregando...";

            // panelAviso
            panelAviso.Location = new Point(25, 225);
            panelAviso.Size = new Size(935, 60);
            panelAviso.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelAviso.Controls.Add(lblAvisoTitulo);
            panelAviso.Controls.Add(lblAvisoDesc);

            lblAvisoTitulo.Location = new Point(15, 8);
            lblAvisoTitulo.Size = new Size(900, 20);
            lblAvisoTitulo.Text = "📅 Agendamentos Integrados";

            lblAvisoDesc.Location = new Point(15, 30);
            lblAvisoDesc.Size = new Size(900, 24);
            lblAvisoDesc.Text = "Acesse a nova aba 'Agendamentos' no menu lateral para visualizar, filtrar e gerenciar todos os atendimentos.";

            // lblSecaoResumo
            lblSecaoResumo.Location = new Point(25, 305);
            lblSecaoResumo.Size = new Size(400, 25);
            lblSecaoResumo.Text = "Catálogo de Serviços da Barbearia";
            lblSecaoResumo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // dgvResumo
            dgvResumo.Location = new Point(25, 335);
            dgvResumo.Size = new Size(935, 270);
            dgvResumo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvResumo.ReadOnly = true;
            dgvResumo.AllowUserToAddRows = false;
            dgvResumo.AllowUserToDeleteRows = false;

            // UserControl
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnAtualizar);
            Controls.Add(lblStatus);
            Controls.Add(cardServicos);
            Controls.Add(cardServicosAtivos);
            Controls.Add(cardClientes);
            Controls.Add(cardBarbeiros);
            Controls.Add(panelAviso);
            Controls.Add(lblSecaoResumo);
            Controls.Add(dgvResumo);

            Size = new Size(980, 630);

            cardServicos.ResumeLayout(false);
            cardServicosAtivos.ResumeLayout(false);
            cardClientes.ResumeLayout(false);
            cardBarbeiros.ResumeLayout(false);
            panelAviso.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResumo).EndInit();
            ResumeLayout(false);
        }
    }
}
