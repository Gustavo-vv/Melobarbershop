namespace Melobarbershop.Desktop.Forms.Usuarios
{
    partial class FormHistoricoCliente
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnFechar;
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
            lblStatus = new Label();
            dgvHistorico = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvHistorico).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Size = new Size(600, 32);
            lblTitulo.Text = "Histórico do Cliente";

            // lblSubtitulo
            lblSubtitulo.Location = new Point(24, 54);
            lblSubtitulo.Size = new Size(600, 22);
            lblSubtitulo.Text = "Todos os agendamentos realizados pelo cliente (mais recentes primeiro)";

            // btnFechar
            btnFechar.Location = new Point(730, 22);
            btnFechar.Size = new Size(110, 36);
            btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFechar.Text = "Fechar";
            btnFechar.Click += btnFechar_Click;

            // lblStatus
            lblStatus.Location = new Point(24, 86);
            lblStatus.Size = new Size(816, 22);
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // dgvHistorico
            dgvHistorico.Location = new Point(24, 114);
            dgvHistorico.Size = new Size(816, 430);
            dgvHistorico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorico.ReadOnly = true;
            dgvHistorico.AllowUserToAddRows = false;
            dgvHistorico.AllowUserToDeleteRows = false;
            dgvHistorico.CellPainting += dgvHistorico_CellPainting;

            // FormHistoricoCliente
            ClientSize = new Size(864, 568);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnFechar);
            Controls.Add(lblStatus);
            Controls.Add(dgvHistorico);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            ((System.ComponentModel.ISupportInitialize)dgvHistorico).EndInit();
            ResumeLayout(false);
        }
    }
}
