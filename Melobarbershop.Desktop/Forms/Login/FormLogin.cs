using Melobarbershop.Desktop.Configuration;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

using Melobarbershop.Desktop.Forms.Dashboard;

namespace Melobarbershop.Desktop.Forms.Login
{
    public partial class FormLogin : Form
    {
        private readonly AuthService _authService = new();

        public FormLogin()
        {
            InitializeComponent();
            ConfigurarEstilo();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;
            this.Font = TemaMelobarbershop.BodyFont;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Melo Barbershop — Acesso Administrativo";
            this.Size = new Size(465, 545);

            picLogo.Image = TemaMelobarbershop.CarregarLogo();

            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

            lblEmail.Font = TemaMelobarbershop.BodyBoldFont;
            lblEmail.ForeColor = TemaMelobarbershop.TextMuted;
            lblSenha.Font = TemaMelobarbershop.BodyBoldFont;
            lblSenha.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarTextBox(txtEmail);
            TemaMelobarbershop.EstilizarTextBox(txtSenha);
            txtSenha.UseSystemPasswordChar = true;

            TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnEntrar);
            lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
            lblStatus.Font = TemaMelobarbershop.SmallBoldFont;
            lblStatus.Text = string.Empty;

            lblApiUrl.Text = $"Conectando em: {AppConfig.ApiBaseUrl}";
            lblApiUrl.ForeColor = TemaMelobarbershop.TextMuted;
            lblApiUrl.Font = TemaMelobarbershop.SmallFont;

            // Pré-preenchimento das credenciais de seed admin para conveniência
            txtEmail.Text = "admin@melobarbershop.com";
            txtSenha.Text = "Admin@123";
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var senha = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                lblStatus.ForeColor = AppTheme.DangerColor;
                lblStatus.Text = "Informe o e-mail e a senha.";
                return;
            }

            btnEntrar.Enabled = false;
            btnEntrar.Text = "Conectando...";
            lblStatus.ForeColor = AppTheme.GoldPrimary;
            lblStatus.Text = "Validando credenciais com a API...";

            try
            {
                var resposta = await _authService.LoginAsync(email, senha);

                if (resposta.Sucesso && resposta.Dados != null)
                {
                    // Valida se o usuário tem a Role Admin
                    var perfis = resposta.Dados.Perfis ?? new List<string>();
                    var ehAdmin = perfis.Any(p => p.Equals("Admin", StringComparison.OrdinalIgnoreCase));

                    if (!ehAdmin)
                    {
                        _authService.Logout();
                        lblStatus.ForeColor = AppTheme.DangerColor;
                        lblStatus.Text = "Acesso negado: Este painel é restrito a administradores.";
                        return;
                    }

                    lblStatus.ForeColor = AppTheme.SuccessColor;
                    lblStatus.Text = "Login efetuado com sucesso! Abrindo painel...";

                    await Task.Delay(300);

                    this.Hide();
                    var mainForm = new FormMain();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    lblStatus.ForeColor = AppTheme.DangerColor;
                    var msg = !string.IsNullOrWhiteSpace(resposta.Mensagem) ? resposta.Mensagem : "Falha na autenticação.";
                    if (resposta.Erros != null && resposta.Erros.Any())
                    {
                        msg += " " + string.Join(" ", resposta.Erros);
                    }
                    lblStatus.Text = msg;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = AppTheme.DangerColor;
                lblStatus.Text = $"Erro de conexão: {ex.Message}";
            }
            finally
            {
                btnEntrar.Enabled = true;
                btnEntrar.Text = "ENTRAR NO SISTEMA";
            }
        }
    }
}
