namespace Melobarbershop.Desktop.Forms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelCard;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblSenha;
        private TextBox txtSenha;
        private Button btnEntrar;
        private Label lblStatus;
        private Label lblApiUrl;

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
            panelCard = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblSenha = new Label();
            txtSenha = new TextBox();
            btnEntrar = new Button();
            lblStatus = new Label();
            lblApiUrl = new Label();

            panelCard.SuspendLayout();
            SuspendLayout();

            // panelCard
            panelCard.Location = new Point(35, 30);
            panelCard.Size = new Size(375, 420);
            panelCard.Controls.Add(lblTitulo);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblEmail);
            panelCard.Controls.Add(txtEmail);
            panelCard.Controls.Add(lblSenha);
            panelCard.Controls.Add(txtSenha);
            panelCard.Controls.Add(btnEntrar);
            panelCard.Controls.Add(lblStatus);
            panelCard.Controls.Add(lblApiUrl);

            // lblTitulo
            lblTitulo.Text = "MELO BARBERSHOP";
            lblTitulo.Location = new Point(20, 25);
            lblTitulo.Size = new Size(335, 35);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitulo
            lblSubtitulo.Text = "Painel Administrativo do Gestor";
            lblSubtitulo.Location = new Point(20, 60);
            lblSubtitulo.Size = new Size(335, 25);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblEmail
            lblEmail.Text = "E-mail de Acesso";
            lblEmail.Location = new Point(25, 105);
            lblEmail.Size = new Size(325, 20);

            // txtEmail
            txtEmail.Location = new Point(25, 128);
            txtEmail.Size = new Size(325, 28);
            txtEmail.Font = new Font("Segoe UI", 10.5F);

            // lblSenha
            lblSenha.Text = "Senha";
            lblSenha.Location = new Point(25, 175);
            lblSenha.Size = new Size(325, 20);

            // txtSenha
            txtSenha.Location = new Point(25, 198);
            txtSenha.Size = new Size(325, 28);
            txtSenha.Font = new Font("Segoe UI", 10.5F);

            // btnEntrar
            btnEntrar.Location = new Point(25, 250);
            btnEntrar.Size = new Size(325, 42);
            btnEntrar.Text = "ENTRAR NO SISTEMA";
            btnEntrar.Click += btnEntrar_Click;

            // lblStatus
            lblStatus.Location = new Point(25, 305);
            lblStatus.Size = new Size(325, 55);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // lblApiUrl
            lblApiUrl.Location = new Point(25, 375);
            lblApiUrl.Size = new Size(325, 20);
            lblApiUrl.TextAlign = ContentAlignment.MiddleCenter;

            // FormLogin
            ClientSize = new Size(445, 480);
            Controls.Add(panelCard);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }
    }
}
