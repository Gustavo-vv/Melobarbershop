namespace Melobarbershop.Desktop.Forms
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelSidebar;
        private Label lblLogo;
        private Label lblLogoSub;
        private Button btnMenuDashboard;
        private Button btnMenuServicos;
        private Button btnMenuUsuarios;
        private Button btnMenuSair;

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
            lblLogo = new Label();
            lblLogoSub = new Label();
            btnMenuDashboard = new Button();
            btnMenuServicos = new Button();
            btnMenuUsuarios = new Button();
            btnMenuSair = new Button();

            panelRodape = new Panel();
            lblUsuarioLogado = new Label();
            lblStatusApi = new Label();

            panelConteudo = new Panel();

            panelSidebar.SuspendLayout();
            panelRodape.SuspendLayout();
            SuspendLayout();

            // panelSidebar
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 240;
            panelSidebar.Controls.Add(lblLogo);
            panelSidebar.Controls.Add(lblLogoSub);
            panelSidebar.Controls.Add(btnMenuDashboard);
            panelSidebar.Controls.Add(btnMenuServicos);
            panelSidebar.Controls.Add(btnMenuUsuarios);
            panelSidebar.Controls.Add(btnMenuSair);

            // lblLogo
            lblLogo.Location = new Point(20, 25);
            lblLogo.Size = new Size(200, 30);
            lblLogo.Text = "MELO BARBERSHOP";

            // lblLogoSub
            lblLogoSub.Location = new Point(20, 55);
            lblLogoSub.Size = new Size(200, 20);
            lblLogoSub.Text = "PAINEL ADMINISTRATIVO";
            lblLogoSub.Font = new Font("Segoe UI", 8F);

            // btnMenuDashboard
            btnMenuDashboard.Location = new Point(0, 110);
            btnMenuDashboard.Size = new Size(240, 50);
            btnMenuDashboard.Text = "📊  Dashboard";
            btnMenuDashboard.Click += btnMenuDashboard_Click;

            // btnMenuServicos
            btnMenuServicos.Location = new Point(0, 165);
            btnMenuServicos.Size = new Size(240, 50);
            btnMenuServicos.Text = "💈  Serviços";
            btnMenuServicos.Click += btnMenuServicos_Click;

            // btnMenuUsuarios
            btnMenuUsuarios.Location = new Point(0, 220);
            btnMenuUsuarios.Size = new Size(240, 50);
            btnMenuUsuarios.Text = "👥  Usuários & Barbeiros";
            btnMenuUsuarios.Click += btnMenuUsuarios_Click;

            // btnMenuSair
            btnMenuSair.Dock = DockStyle.Bottom;
            btnMenuSair.Height = 55;
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
