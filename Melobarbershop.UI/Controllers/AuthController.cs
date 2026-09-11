using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IHttpClientFactory httpClientFactory, ILogger<AuthController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

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

                // Regra de Redirecionamento por Role/Claim:
                // SE Role = "Admin": /Dashboard (mapeado para Admin/Index)
                // SE Role = "Cliente": /Home/Index
                // Fallback: /Home/Index
                string redirectUrl = "/Home/Index";
                if (loginData.Perfis != null && loginData.Perfis.Contains("Admin", StringComparer.OrdinalIgnoreCase))
                {
                    redirectUrl = "/Dashboard";
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

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(string nome, string email, string password)
        {
            // Lógica de registro a ser implementada futuramente
            return RedirectToAction("Login");
        }
    }
}

