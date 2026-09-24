namespace Melobarbershop.Desktop.Forms.Usuarios
{
    partial class FormEditarCliente
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblNome;
        private TextBox txtNome;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblTelefone;
        private TextBox txtTelefone;
        private Label lblDataNascimento;
        private DateTimePicker dtpDataNascimento;
        private Label lblNotas;
        private TextBox txtNotas;
        private Button btnSalvar;
        private Button btnCancelar;

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
            lblNome = new Label();
            txtNome = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTelefone = new Label();
            txtTelefone = new TextBox();
            lblDataNascimento = new Label();
            dtpDataNascimento = new DateTimePicker();
            lblNotas = new Label();
            txtNotas = new TextBox();
            btnSalvar = new Button();
            btnCancelar = new Button();

            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(430, 30);
            lblTitulo.Text = "Editar Dados do Cliente";

            // lblNome
            lblNome.Text = "Nome Completo *";
            lblNome.Location = new Point(25, 65);
            lblNome.Size = new Size(430, 20);

            // txtNome
            txtNome.Location = new Point(25, 88);
            txtNome.Size = new Size(430, 28);

            // lblEmail
            lblEmail.Text = "E-mail (Identificador / Não editável)";
            lblEmail.Location = new Point(25, 125);
            lblEmail.Size = new Size(430, 20);

            // txtEmail
            txtEmail.Location = new Point(25, 148);
            txtEmail.Size = new Size(430, 28);
            txtEmail.ReadOnly = true;

            // lblTelefone
            lblTelefone.Text = "Telefone / WhatsApp";
            lblTelefone.Location = new Point(25, 185);
            lblTelefone.Size = new Size(205, 20);

            // txtTelefone
            txtTelefone.Location = new Point(25, 208);
            txtTelefone.Size = new Size(205, 28);

            // lblDataNascimento
            lblDataNascimento.Text = "Data de Nascimento";
            lblDataNascimento.Location = new Point(250, 185);
            lblDataNascimento.Size = new Size(205, 20);

            // dtpDataNascimento
            dtpDataNascimento.Location = new Point(250, 208);
            dtpDataNascimento.Size = new Size(205, 28);
            dtpDataNascimento.Format = DateTimePickerFormat.Custom;
            dtpDataNascimento.CustomFormat = "dd/MM/yyyy";
            dtpDataNascimento.ShowCheckBox = true;

            // lblNotas
            lblNotas.Text = "Preferências / Observações";
            lblNotas.Location = new Point(25, 248);
            lblNotas.Size = new Size(430, 20);

            // txtNotas
            txtNotas.Location = new Point(25, 271);
            txtNotas.Size = new Size(430, 75);
            txtNotas.Multiline = true;
            txtNotas.ScrollBars = ScrollBars.Vertical;

            // btnSalvar
            btnSalvar.Location = new Point(195, 365);
            btnSalvar.Size = new Size(130, 38);
            btnSalvar.Text = "Salvar";
            btnSalvar.Click += btnSalvar_Click;

            // btnCancelar
            btnCancelar.Location = new Point(335, 365);
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            // Form
            ClientSize = new Size(480, 425);
            Controls.Add(lblTitulo);
            Controls.Add(lblNome);
            Controls.Add(txtNome);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblTelefone);
            Controls.Add(txtTelefone);
            Controls.Add(lblDataNascimento);
            Controls.Add(dtpDataNascimento);
            Controls.Add(lblNotas);
            Controls.Add(txtNotas);
            Controls.Add(btnSalvar);
            Controls.Add(btnCancelar);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
