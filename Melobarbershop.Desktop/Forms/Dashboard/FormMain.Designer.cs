namespace Melobarbershop.Desktop.Forms.Dashboard
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelSidebar;
        private PictureBox picLogoSidebar;
        private Label lblLogo;
        private Label lblLogoSub;
        private Guna.UI2.WinForms.Guna2Button btnMenuDashboard;
        private Guna.UI2.WinForms.Guna2Button btnMenuAgendamentos;
        private Guna.UI2.WinForms.Guna2Button btnMenuServicos;
        private Guna.UI2.WinForms.Guna2Button btnMenuUsuarios;
        private Guna.UI2.WinForms.Guna2Button btnAlternarTema;
        private Guna.UI2.WinForms.Guna2Button btnMenuSair;

        private Panel panelRodape;
        private Label lblUsuarioLogado;
        private Label lblStatusApi;

        private Panel panelConteudo;

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
            panelSidebar = new Panel();
            picLogoSidebar = new PictureBox();
            lblLogo = new Label();
            lblLogoSub = new Label();
            btnMenuDashboard = new Guna.UI2.WinForms.Guna2Button();
            btnMenuAgendamentos = new Guna.UI2.WinForms.Guna2Button();
            btnMenuServicos = new Guna.UI2.WinForms.Guna2Button();
            btnMenuUsuarios = new Guna.UI2.WinForms.Guna2Button();
            btnAlternarTema = new Guna.UI2.WinForms.Guna2Button();
            btnMenuSair = new Guna.UI2.WinForms.Guna2Button();

            panelRodape = new Panel();
            lblUsuarioLogado = new Label();
            lblStatusApi = new Label();

            panelConteudo = new Panel();

            panelSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoSidebar).BeginInit();
            panelRodape.SuspendLayout();
            SuspendLayout();

            // panelSidebar
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 250;
            panelSidebar.Controls.Add(picLogoSidebar);
            panelSidebar.Controls.Add(lblLogo);
            panelSidebar.Controls.Add(lblLogoSub);
            panelSidebar.Controls.Add(btnMenuDashboard);
            panelSidebar.Controls.Add(btnMenuAgendamentos);
            panelSidebar.Controls.Add(btnMenuServicos);
            panelSidebar.Controls.Add(btnMenuUsuarios);
            panelSidebar.Controls.Add(btnAlternarTema);
            panelSidebar.Controls.Add(btnMenuSair);

            // picLogoSidebar
            picLogoSidebar.Location = new Point(20, 18);
            picLogoSidebar.Size = new Size(50, 45);
            picLogoSidebar.SizeMode = PictureBoxSizeMode.Zoom;

            // lblLogo
            lblLogo.Location = new Point(78, 20);
            lblLogo.Size = new Size(165, 24);
            lblLogo.Text = "MELO BARBERSHOP";

            // lblLogoSub
            lblLogoSub.Location = new Point(78, 44);
            lblLogoSub.Size = new Size(165, 18);
            lblLogoSub.Text = "PAINEL ADMINISTRATIVO";
            lblLogoSub.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);

            // btnMenuDashboard
            btnMenuDashboard.Location = new Point(0, 110);
            btnMenuDashboard.Size = new Size(240, 50);
            btnMenuDashboard.Text = "📊  Dashboard";
            btnMenuDashboard.Click += btnMenuDashboard_Click;

            // btnMenuAgendamentos
            btnMenuAgendamentos.Location = new Point(0, 165);
            btnMenuAgendamentos.Size = new Size(240, 50);
            btnMenuAgendamentos.Text = "📅  Agendamentos";
            btnMenuAgendamentos.Click += btnMenuAgendamentos_Click;

            // btnMenuServicos
            btnMenuServicos.Location = new Point(0, 220);
            btnMenuServicos.Size = new Size(240, 50);
            btnMenuServicos.Text = "💈  Serviços";
            btnMenuServicos.Click += btnMenuServicos_Click;

            // btnMenuUsuarios
            btnMenuUsuarios.Location = new Point(0, 275);
            btnMenuUsuarios.Size = new Size(240, 50);
            btnMenuUsuarios.Text = "👥  Usuários & Barbeiros";
            btnMenuUsuarios.Click += btnMenuUsuarios_Click;

            // btnAlternarTema
            btnAlternarTema.Dock = DockStyle.Bottom;
            btnAlternarTema.Height = 45;
            btnAlternarTema.Text = "☀️  Modo Claro";
            btnAlternarTema.Click += btnAlternarTema_Click;

            // btnMenuSair
            btnMenuSair.Dock = DockStyle.Bottom;
            btnMenuSair.Height = 50;
            btnMenuSair.Text = "🚪  Sair do Sistema";
            btnMenuSair.Click += btnMenuSair_Click;

            // panelRodape
            panelRodape.Dock = DockStyle.Bottom;
            panelRodape.Height = 35;
            panelRodape.Controls.Add(lblUsuarioLogado);
            panelRodape.Controls.Add(lblStatusApi);

            // lblUsuarioLogado
            lblUsuarioLogado.Location = new Point(15, 8);
            lblUsuarioLogado.Size = new Size(450, 20);
            lblUsuarioLogado.Text = "👤 Gestor: -";

            // lblStatusApi
            lblStatusApi.Location = new Point(500, 8);
            lblStatusApi.Size = new Size(450, 20);
            lblStatusApi.TextAlign = ContentAlignment.MiddleRight;
            lblStatusApi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatusApi.Text = "🟢 API Conectada";

            // panelConteudo
            panelConteudo.Dock = DockStyle.Fill;

            // FormMain
            ClientSize = new Size(1220, 720);
            Controls.Add(panelConteudo);
            Controls.Add(panelRodape);
            Controls.Add(panelSidebar);

            panelSidebar.ResumeLayout(false);
            panelRodape.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
