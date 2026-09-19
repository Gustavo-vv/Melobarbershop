// ============================================================================
// Arquivo: AuthController.cs
// Camada: Melobarbershop.UI (Controllers)
// Objetivo: Controlador de autenticação, login de clientes/administradores, cookies de sessão e cadastro.
// Papel na Arquitetura:
//   - Recebe as credenciais do usuário e valida contra o endpoint de API /api/Auth/login.
//   - Cria a identidade do usuário na aplicação Web através de CookieAuthentication (com persistência do token JWT).
//   - Roteia o usuário para a área administrativa (/Admin) ou área pública (/Home) conforme suas roles.
// ============================================================================

using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.UI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers;

/// <summary>
/// Controlador responsável pelas telas e fluxos de login, logout e cadastro de usuários no portal Web.
/// </summary>
public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Construtor com injeção de HttpClientFactory e do serviço de logs.
    /// </summary>
    public AuthController(IHttpClientFactory httpClientFactory, ILogger<AuthController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Renderiza a tela de login. Redireciona o usuário caso ele já esteja autenticado.
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    /// <summary>
    /// Processa o formulário de login consumindo a API interna e emitindo o cookie de autenticação local.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
        {
            return BadRequest(new { sucesso = false, mensagem = "Por favor, preencha o e-mail e a senha." });
        }

        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/Auth/login", jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResult = JsonSerializer.Deserialize<ApiResposta<LoginRespostaDto>>(responseBody, jsonOptions);

            if (!response.IsSuccessStatusCode || apiResult == null || !apiResult.Sucesso || apiResult.Dados == null)
            {
                var msg = apiResult?.Mensagem;
                if (string.IsNullOrWhiteSpace(msg))
                {
                    msg = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        ? "Email ou senha inválidos."
                        : "Erro ao autenticar. Tente novamente mais tarde.";
                }

                return StatusCode((int)response.StatusCode, new { sucesso = false, mensagem = msg });
            }

            var loginData = apiResult.Dados;

            // Claims da autenticação local por Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginData.NomeUsuario ?? string.Empty),
                new Claim(ClaimTypes.Email, loginData.Email ?? string.Empty),
                new Claim("jwt_token", loginData.Token ?? string.Empty)
            };

            if (!string.IsNullOrEmpty(loginData.FotoPerfilUrl))
            {
                claims.Add(new Claim("foto_url", loginData.FotoPerfilUrl));
            }

            if (loginData.Perfis != null)
            {
                foreach (var role in loginData.Perfis)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = loginData.Expiracao > DateTime.UtcNow
                    ? loginData.Expiracao
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Regra de Redirecionamento por Role: Admin -> /Admin; Demais -> /Home/Index
            string redirectUrl = "/Home/Index";
            if (loginData.Perfis != null && loginData.Perfis.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            {
                redirectUrl = "/Admin";
            }

            return Ok(new
            {
                sucesso = true,
                mensagem = apiResult.Mensagem ?? "Login realizado com sucesso!",
                redirectUrl,
                token = loginData.Token,
                nome = loginData.NomeUsuario,
                roles = loginData.Perfis
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão com a API de autenticação.");
            return StatusCode(503, new
            {
                sucesso = false,
                mensagem = "Não foi possível conectar ao servidor de autenticação. Verifique se a API está online."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado durante o login.");
            return StatusCode(500, new
            {
                sucesso = false,
                mensagem = "Ocorreu um erro interno ao processar o login."
            });
        }
    }

    /// <summary>
    /// Finaliza a sessão do usuário (Logout via GET) revogando os cookies locais.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Finaliza a sessão do usuário via requisição POST segura contra CSRF.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Renderiza a página de Termos de Uso da plataforma.
    /// </summary>
    [HttpGet]
    public IActionResult Termos() => View();

    /// <summary>
    /// Renderiza a página de Políticas de Privacidade.
    /// </summary>
    [HttpGet]
    public IActionResult Privacidade() => View();

    /// <summary>
    /// Renderiza a página com formulário de cadastro de novo cliente.
    /// </summary>
    [HttpGet]
    public IActionResult Cadastro()
    {
        var viewModel = new CadastroViewModel();
        return View(viewModel);
    }

    /// <summary>
    /// Processa o cadastro de novo cliente enviando os dados para a API interna.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Cadastro([FromBody] CriarUsuarioDto dto)
    {
        if (dto == null)
            return BadRequest(new { sucesso = false, mensagem = "Dados inválidos. Por favor, preencha todos os campos." });

        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/Auth/registrar", jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResult = JsonSerializer.Deserialize<ApiResposta<object>>(responseBody, jsonOptions);

            if (response.IsSuccessStatusCode && apiResult?.Sucesso == true)
            {
                return StatusCode(201, new
                {
                    sucesso = true,
                    mensagem = apiResult.Mensagem ?? "Conta criada com sucesso! Faça o login para continuar."
                });
            }

            var mensagem = apiResult?.Mensagem ?? "Erro ao criar conta. Tente novamente.";
            if (apiResult?.Erros != null && apiResult.Erros.Count > 0)
                mensagem = string.Join(" ", apiResult.Erros);

            return StatusCode((int)response.StatusCode, new { sucesso = false, mensagem });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão com a API de registro.");
            return StatusCode(503, new
            {
                sucesso = false,
                mensagem = "Não foi possível conectar ao servidor. Verifique se a API está online."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado durante o cadastro.");
            return StatusCode(500, new
            {
                sucesso = false,
                mensagem = "Ocorreu um erro interno ao processar o cadastro."
            });
        }
    }
}
