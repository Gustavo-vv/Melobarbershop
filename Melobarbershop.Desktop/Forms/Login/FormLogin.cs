// ============================================================================
// Arquivo: FormLogin.cs
// Camada: Melobarbershop.Desktop (Forms / Login)
// Objetivo: Tela de autenticação e acesso exclusivo para operadores administradores do sistema.
// Papel na Arquitetura:
//   - Coleta credenciais (E-mail e Senha) e consome AuthService para autenticação contra a API.
//   - Impede o acesso de usuários que não possuam a Role "Admin".
//   - Redireciona para o formulário principal (FormMain) após login bem-sucedido.
// ============================================================================

using Melobarbershop.Desktop.Configuration;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;
using Melobarbershop.Desktop.Forms.Dashboard;

namespace Melobarbershop.Desktop.Forms.Login;

/// <summary>
/// Formulário de login do aplicativo desktop administrativo.
/// </summary>
public partial class FormLogin : Form
{
    private readonly AuthService _authService = new();

    /// <summary>
    /// Construtor padrão da tela de login.
    /// </summary>
    public FormLogin()
    {
        InitializeComponent();
        ConfigurarEstilo();

        TemaMelobarbershop.TemaAlterado += AplicarTema;
    }

    /// <summary>
    /// Configura fontes, tamanhos, posições e dados pré-preenchidos de inicialização.
    /// </summary>
    private void ConfigurarEstilo()
    {
        this.Font = TemaMelobarbershop.BodyFont;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Melo Barbershop — Acesso Administrativo";
        this.Size = new Size(465, 545);

        picLogo.Image = TemaMelobarbershop.CarregarLogo();

        lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
        lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
        lblEmail.Font = TemaMelobarbershop.BodyBoldFont;
        lblSenha.Font = TemaMelobarbershop.BodyBoldFont;
        txtSenha.UseSystemPasswordChar = true;
        lblStatus.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatus.Text = string.Empty;

        lblApiUrl.Text = $"Conectando em: {AppConfig.ApiBaseUrl}";
        lblApiUrl.Font = TemaMelobarbershop.SmallFont;

        // Pré-preenchimento das credenciais de seed admin para conveniência
        txtEmail.Text = "admin@melobarbershop.com";
        txtSenha.Text = "Admin@123";

        AplicarTema();
    }

    /// <summary>
    /// Aplica as cores e estilos do tema configurado nos controles da tela de login.
    /// </summary>
    public void AplicarTema()
    {
        this.BackColor = TemaMelobarbershop.BackgroundDark;
        this.ForeColor = TemaMelobarbershop.TextPrimary;

        lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
        lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
        lblEmail.ForeColor = TemaMelobarbershop.TextMuted;
        lblSenha.ForeColor = TemaMelobarbershop.TextMuted;
        lblApiUrl.ForeColor = TemaMelobarbershop.TextMuted;

        TemaMelobarbershop.EstilizarGunaPanelCard(panelCard);
        TemaMelobarbershop.EstilizarGunaTextBox(txtEmail);
        TemaMelobarbershop.EstilizarGunaTextBox(txtSenha);
        TemaMelobarbershop.EstilizarGunaButtonPrimario(btnEntrar);
        lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
    }

    /// <summary>
    /// Desinscreve os eventos globais ao fechar o formulário.
    /// </summary>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        TemaMelobarbershop.TemaAlterado -= AplicarTema;
        base.OnFormClosed(e);
    }

    /// <summary>
    /// Evento de clique no botão de login. Valida credenciais e verifica perfil de administrador.
    /// </summary>
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
