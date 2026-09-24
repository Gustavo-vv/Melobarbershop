using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios
{
    public partial class FormEditarCliente : Form
    {
        public AtualizarDadosClienteDto? UsuarioAtualizado { get; private set; }
        public bool Salvo { get; private set; } = false;

        private readonly UsuarioDto _usuarioOriginal;

        public FormEditarCliente(UsuarioDto usuario)
        {
            _usuarioOriginal = usuario ?? throw new ArgumentNullException(nameof(usuario));

            InitializeComponent();
            ConfigurarEstilo();
            PreencherCampos();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;
            this.Font = TemaMelobarbershop.BodyFont;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 470);
            this.Text = "Editar Dados do Cliente";

            lblTitulo.Font = TemaMelobarbershop.SectionHeadingFont;
            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblTitulo.Text = "Editar Informações do Cliente";

            txtNome.BackColor = TemaMelobarbershop.SurfaceSecondary;
            txtNome.ForeColor = TemaMelobarbershop.TextPrimary;
            txtNome.BorderStyle = BorderStyle.FixedSingle;

            txtEmail.BackColor = TemaMelobarbershop.BackgroundDark;
            txtEmail.ForeColor = TemaMelobarbershop.TextMuted;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;

            txtTelefone.BackColor = TemaMelobarbershop.SurfaceSecondary;
            txtTelefone.ForeColor = TemaMelobarbershop.TextPrimary;
            txtTelefone.BorderStyle = BorderStyle.FixedSingle;

            dtpDataNascimento.CalendarMonthBackground = TemaMelobarbershop.SurfaceSecondary;
            dtpDataNascimento.CalendarForeColor = TemaMelobarbershop.TextPrimary;
            dtpDataNascimento.MaxDate = DateTime.Today;

            txtNotas.BackColor = TemaMelobarbershop.SurfaceSecondary;
            txtNotas.ForeColor = TemaMelobarbershop.TextPrimary;
            txtNotas.BorderStyle = BorderStyle.FixedSingle;

            TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnSalvar);
            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnCancelar);
        }

        private void PreencherCampos()
        {
            txtNome.Text = _usuarioOriginal.Nome;
            txtEmail.Text = _usuarioOriginal.Email;
            txtTelefone.Text = _usuarioOriginal.PhoneNumber ?? string.Empty;

            if (_usuarioOriginal.DataNascimento.HasValue)
            {
                var dt = _usuarioOriginal.DataNascimento.Value.Date;
                if (dt <= DateTime.Today && dt >= dtpDataNascimento.MinDate)
                {
                    dtpDataNascimento.Value = dt;
                    dtpDataNascimento.Checked = true;
                }
                else
                {
                    dtpDataNascimento.Checked = false;
                }
            }
            else
            {
                dtpDataNascimento.Checked = false;
            }

            txtNotas.Text = _usuarioOriginal.PreferenciasNotas ?? string.Empty;
        }

        private void btnSalvar_Click(object? sender, EventArgs e)
        {
            var nome = txtNome.Text.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("O nome do cliente é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            DateTime? dataNasc = null;
            if (dtpDataNascimento.Checked)
            {
                dataNasc = dtpDataNascimento.Value.Date;
            }

            UsuarioAtualizado = new AtualizarDadosClienteDto
            {
                Nome = nome,
                Telefone = string.IsNullOrWhiteSpace(txtTelefone.Text) ? null : txtTelefone.Text.Trim(),
                DataNascimento = dataNasc,
                PreferenciasNotas = string.IsNullOrWhiteSpace(txtNotas.Text) ? null : txtNotas.Text.Trim()
            };

            Salvo = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
