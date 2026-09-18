using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios
{
    partial class FormHistoricoCliente
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnFechar;

        // Seção de Perfil
        private CardPanel pnlPerfil;
        private Label lblPerfilTitulo;
        private Label lblNomeValor;
        private Label lblEmailValor;
        private Label lblTelefoneValor;
        private Label lblNascimentoValor;
        private Label lblCadastroValor;
        private Label lblStatusBadge;
        private Label lblPreferenciasValor;

        private Label lblStatus;
        private DataGridView dgvHistorico;

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
            btnFechar = new Button();

            pnlPerfil = new CardPanel();
            lblPerfilTitulo = new Label();
            lblNomeValor = new Label();
            lblEmailValor = new Label();
            lblTelefoneValor = new Label();
            lblNascimentoValor = new Label();
            lblCadastroValor = new Label();
            lblStatusBadge = new Label();
            lblPreferenciasValor = new Label();

            lblStatus = new Label();
            dgvHistorico = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvHistorico).BeginInit();
            pnlPerfil.SuspendLayout();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(24, 16);
            lblTitulo.Size = new Size(650, 32);
            lblTitulo.Text = "Histórico do Cliente";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(24, 48);
            lblSubtitulo.Size = new Size(650, 20);
            lblSubtitulo.Text = "Dados cadastrais e histórico de agendamentos do cliente";

            // btnFechar
            btnFechar.Location = new Point(730, 18);
            btnFechar.Size = new Size(110, 36);
            btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFechar.Text = "Fechar";
            btnFechar.Click += btnFechar_Click;

            // pnlPerfil (CardPanel)
            pnlPerfil.Location = new Point(24, 76);
            pnlPerfil.Size = new Size(816, 155);
            pnlPerfil.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblPerfilTitulo
            lblPerfilTitulo.Location = new Point(16, 12);
            lblPerfilTitulo.Size = new Size(200, 22);
            lblPerfilTitulo.Text = "DADOS DO CLIENTE";

            // lblStatusBadge (tag Ativo/Inativo no topo direito do card)
            lblStatusBadge.Location = new Point(700, 12);
            lblStatusBadge.Size = new Size(95, 24);
            lblStatusBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatusBadge.TextAlign = ContentAlignment.MiddleCenter;

            // Linha 1 de dados: Nome, Email, Telefone
            lblNomeValor.Location = new Point(16, 42);
            lblNomeValor.Size = new Size(250, 22);

            lblEmailValor.Location = new Point(280, 42);
            lblEmailValor.Size = new Size(270, 22);

            lblTelefoneValor.Location = new Point(565, 42);
            lblTelefoneValor.Size = new Size(230, 22);

            // Linha 2 de dados: Data de Nascimento, Data de Cadastro
            lblNascimentoValor.Location = new Point(16, 70);
            lblNascimentoValor.Size = new Size(250, 22);

            lblCadastroValor.Location = new Point(280, 70);
            lblCadastroValor.Size = new Size(270, 22);

            // Linha 3 de dados: Preferências / Observações (opcional)
            lblPreferenciasValor.Location = new Point(16, 98);
            lblPreferenciasValor.Size = new Size(780, 42);

            pnlPerfil.Controls.Add(lblPerfilTitulo);
            pnlPerfil.Controls.Add(lblStatusBadge);
            pnlPerfil.Controls.Add(lblNomeValor);
            pnlPerfil.Controls.Add(lblEmailValor);
            pnlPerfil.Controls.Add(lblTelefoneValor);
            pnlPerfil.Controls.Add(lblNascimentoValor);
            pnlPerfil.Controls.Add(lblCadastroValor);
            pnlPerfil.Controls.Add(lblPreferenciasValor);

            // lblStatus (status da listagem de agendamentos)
            lblStatus.Location = new Point(24, 240);
            lblStatus.Size = new Size(816, 22);
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // dgvHistorico
            dgvHistorico.Location = new Point(24, 266);
            dgvHistorico.Size = new Size(816, 380);
            dgvHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorico.ReadOnly = true;
            dgvHistorico.AllowUserToAddRows = false;
            dgvHistorico.AllowUserToDeleteRows = false;
            dgvHistorico.CellPainting += dgvHistorico_CellPainting;

            // FormHistoricoCliente
            ClientSize = new Size(864, 668);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnFechar);
            Controls.Add(pnlPerfil);
            Controls.Add(lblStatus);
            Controls.Add(dgvHistorico);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            pnlPerfil.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorico).EndInit();
            ResumeLayout(false);
        }
    }
}
