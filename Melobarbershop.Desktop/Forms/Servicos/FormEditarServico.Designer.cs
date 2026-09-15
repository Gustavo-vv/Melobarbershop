namespace Melobarbershop.Desktop.Forms.Servicos
{
    partial class FormEditarServico
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblNome;
        private TextBox txtNome;
        private Label lblDescricao;
        private TextBox txtDescricao;
        private Label lblPreco;
        private NumericUpDown nudPreco;
        private Label lblDuracao;
        private NumericUpDown nudDuracao;
        private CheckBox chkAtivo;
        private CheckBox chkExibirSite;
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
            lblDescricao = new Label();
            txtDescricao = new TextBox();
            lblPreco = new Label();
            nudPreco = new NumericUpDown();
            lblDuracao = new Label();
            nudDuracao = new NumericUpDown();
            chkAtivo = new CheckBox();
            chkExibirSite = new CheckBox();
            btnSalvar = new Button();
            btnCancelar = new Button();

            ((System.ComponentModel.ISupportInitialize)nudPreco).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDuracao).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Location = new Point(25, 20);
            lblTitulo.Size = new Size(430, 30);

            // lblNome
            lblNome.Text = "Nome do Serviço *";
            lblNome.Location = new Point(25, 65);
            lblNome.Size = new Size(430, 20);

            // txtNome
            txtNome.Location = new Point(25, 88);
            txtNome.Size = new Size(430, 28);

            // lblDescricao
            lblDescricao.Text = "Descrição Detalhada";
            lblDescricao.Location = new Point(25, 125);
            lblDescricao.Size = new Size(430, 20);

            // txtDescricao
            txtDescricao.Location = new Point(25, 148);
            txtDescricao.Size = new Size(430, 60);
            txtDescricao.Multiline = true;

            // lblPreco
            lblPreco.Text = "Preço (R$) *";
            lblPreco.Location = new Point(25, 220);
            lblPreco.Size = new Size(200, 20);

            // nudPreco
            nudPreco.Location = new Point(25, 243);
            nudPreco.Size = new Size(200, 28);
            nudPreco.DecimalPlaces = 2;
            nudPreco.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });

            // lblDuracao
            lblDuracao.Text = "Duração (Minutos) *";
            lblDuracao.Location = new Point(255, 220);
            lblDuracao.Size = new Size(200, 20);

            // nudDuracao
            nudDuracao.Location = new Point(255, 243);
            nudDuracao.Size = new Size(200, 28);
            nudDuracao.Maximum = new decimal(new int[] { 480, 0, 0, 0 });
            nudDuracao.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudDuracao.Increment = 5;

            // chkAtivo
            chkAtivo.Text = "Serviço Ativo";
            chkAtivo.Location = new Point(25, 290);
            chkAtivo.Size = new Size(200, 24);

            // chkExibirSite
            chkExibirSite.Text = "Exibir no Site de Agendamento";
            chkExibirSite.Location = new Point(255, 290);
            chkExibirSite.Size = new Size(220, 24);

            // btnSalvar
            btnSalvar.Location = new Point(195, 360);
            btnSalvar.Size = new Size(130, 38);
            btnSalvar.Text = "Salvar";
            btnSalvar.Click += btnSalvar_Click;

            // btnCancelar
            btnCancelar.Location = new Point(335, 360);
            btnCancelar.Size = new Size(120, 38);
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            // Form
            ClientSize = new Size(480, 430);
            Controls.Add(lblTitulo);
            Controls.Add(lblNome);
            Controls.Add(txtNome);
            Controls.Add(lblDescricao);
            Controls.Add(txtDescricao);
            Controls.Add(lblPreco);
            Controls.Add(nudPreco);
            Controls.Add(lblDuracao);
            Controls.Add(nudDuracao);
            Controls.Add(chkAtivo);
            Controls.Add(chkExibirSite);
            Controls.Add(btnSalvar);
            Controls.Add(btnCancelar);

            ((System.ComponentModel.ISupportInitialize)nudPreco).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDuracao).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
