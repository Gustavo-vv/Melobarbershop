namespace Melobarbershop.Desktop.Forms.Login
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;
        private Melobarbershop.Desktop.Theme.CardPanel panelCard;
        private PictureBox picLogo;
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
            panelCard = new Melobarbershop.Desktop.Theme.CardPanel();
            picLogo = new PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();

            // panelCard
            panelCard.Location = new Point(35, 20);
            panelCard.Size = new Size(375, 475);
            panelCard.Controls.Add(picLogo);
            panelCard.Controls.Add(lblTitulo);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblEmail);
            panelCard.Controls.Add(txtEmail);
            panelCard.Controls.Add(lblSenha);
            panelCard.Controls.Add(txtSenha);
            panelCard.Controls.Add(btnEntrar);
            panelCard.Controls.Add(lblStatus);
            panelCard.Controls.Add(lblApiUrl);

            // picLogo
            picLogo.Location = new Point(137, 18);
            picLogo.Size = new Size(100, 65);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;

            // lblTitulo
            lblTitulo.Text = "MELO BARBERSHOP";
            lblTitulo.Location = new Point(20, 88);
            lblTitulo.Size = new Size(335, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitulo
            lblSubtitulo.Text = "Painel Administrativo do Gestor";
            lblSubtitulo.Location = new Point(20, 118);
            lblSubtitulo.Size = new Size(335, 22);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblEmail
            lblEmail.Text = "E-mail de Acesso";
            lblEmail.Location = new Point(25, 150);
            lblEmail.Size = new Size(325, 20);

            // txtEmail
            txtEmail.Location = new Point(25, 172);
            txtEmail.Size = new Size(325, 28);
            txtEmail.Font = new Font("Segoe UI", 10.5F);

            // lblSenha
            lblSenha.Text = "Senha";
            lblSenha.Location = new Point(25, 212);
            lblSenha.Size = new Size(325, 20);

            // txtSenha
            txtSenha.Location = new Point(25, 234);
            txtSenha.Size = new Size(325, 28);
            txtSenha.Font = new Font("Segoe UI", 10.5F);

            // btnEntrar
            btnEntrar.Location = new Point(25, 285);
            btnEntrar.Size = new Size(325, 42);
            btnEntrar.Text = "ENTRAR NO SISTEMA";
            btnEntrar.Click += btnEntrar_Click;

            // lblStatus
            lblStatus.Location = new Point(25, 335);
            lblStatus.Size = new Size(325, 50);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // lblApiUrl
            lblApiUrl.Location = new Point(25, 395);
            lblApiUrl.Size = new Size(325, 20);
            lblApiUrl.TextAlign = ContentAlignment.MiddleCenter;

            // FormLogin
            ClientSize = new Size(445, 520);
            Controls.Add(panelCard);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }
    }
}
