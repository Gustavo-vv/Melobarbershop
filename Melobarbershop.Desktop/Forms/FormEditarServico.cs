using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms
{
    public partial class FormEditarServico : Form
    {
        public CriarServicoDto? ServicoCriado { get; private set; }
        public AtualizarServicoDto? ServicoAtualizado { get; private set; }
        public bool Salvo { get; private set; } = false;

        private readonly ServicoDto? _servicoOriginal;
        private readonly bool _modoEdicao;

        public FormEditarServico(ServicoDto? servico = null)
        {
            _servicoOriginal = servico;
            _modoEdicao = servico != null;

            InitializeComponent();
            ConfigurarEstilo();
            PreencherCampos();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppTheme.BackgroundDark;
            this.ForeColor = AppTheme.TextPrimary;
            this.Font = AppTheme.NormalFont;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 480);
            this.Text = _modoEdicao ? "Editar Serviço" : "Novo Serviço";

            lblTitulo.Font = AppTheme.SubtitleFont;
            lblTitulo.ForeColor = AppTheme.GoldPrimary;
            lblTitulo.Text = _modoEdicao ? "Editar Informações do Serviço" : "Cadastrar Novo Serviço";

            txtNome.BackColor = AppTheme.InputBackground;
            txtNome.ForeColor = AppTheme.TextPrimary;
            txtNome.BorderStyle = BorderStyle.FixedSingle;

            txtDescricao.BackColor = AppTheme.InputBackground;
            txtDescricao.ForeColor = AppTheme.TextPrimary;
            txtDescricao.BorderStyle = BorderStyle.FixedSingle;

            nudPreco.BackColor = AppTheme.InputBackground;
            nudPreco.ForeColor = AppTheme.TextPrimary;

            nudDuracao.BackColor = AppTheme.InputBackground;
            nudDuracao.ForeColor = AppTheme.TextPrimary;

            chkAtivo.ForeColor = AppTheme.TextPrimary;
            chkExibirSite.ForeColor = AppTheme.TextPrimary;

            AppTheme.AplicarEstiloBotaoPrimario(btnSalvar);
            AppTheme.AplicarEstiloBotaoSecundario(btnCancelar);

            if (!_modoEdicao)
            {
                chkAtivo.Visible = false; // Na criação a API não tem campo Ativo no CriarServicoDto
            }
        }

        private void PreencherCampos()
        {
            if (_servicoOriginal != null)
            {
                txtNome.Text = _servicoOriginal.Nome;
                txtDescricao.Text = _servicoOriginal.Descricao ?? string.Empty;
                nudPreco.Value = _servicoOriginal.Preco;
                nudDuracao.Value = _servicoOriginal.DuracaoMinutos;
                chkAtivo.Checked = _servicoOriginal.Ativo;
                chkExibirSite.Checked = _servicoOriginal.ExibirNoSite;
            }
            else
            {
                nudPreco.Value = 40.00m;
                nudDuracao.Value = 30;
                chkAtivo.Checked = true;
                chkExibirSite.Checked = true;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var nome = txtNome.Text.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("O nome do serviço é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (nudPreco.Value <= 0)
            {
                MessageBox.Show("O preço deve ser maior que zero.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudPreco.Focus();
                return;
            }

            if (_modoEdicao)
            {
                ServicoAtualizado = new AtualizarServicoDto
                {
                    Nome = nome,
                    Descricao = string.IsNullOrWhiteSpace(txtDescricao.Text) ? null : txtDescricao.Text.Trim(),
                    Preco = nudPreco.Value,
                    DuracaoMinutos = (int)nudDuracao.Value,
                    Ativo = chkAtivo.Checked,
                    ExibirNoSite = chkExibirSite.Checked
                };
            }
            else
            {
                ServicoCriado = new CriarServicoDto
                {
                    Nome = nome,
                    Descricao = string.IsNullOrWhiteSpace(txtDescricao.Text) ? null : txtDescricao.Text.Trim(),
                    Preco = nudPreco.Value,
                    DuracaoMinutos = (int)nudDuracao.Value,
                    ExibirNoSite = chkExibirSite.Checked
                };
            }

            Salvo = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
