using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SenacFlix.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private JwtSecurityToken GerarToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Chave"]!));
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Emissor"],
                    audience: _configuration["Jwt:Audiencia"],
                    expires: DateTime.Now.AddHours(8),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] CriarUsuarioDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null) return BadRequest(ApiResposta<object>.Falha("Já existe um usuário com este email! "));

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nome = dto.Nome,
                PhoneNumber = dto.TelefoneWhatsApp,
                DataNascimento = dto.DataNascimento,
                Ativo = true,
                DataCadastro = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResposta<object>.FalhaValidacao(errors, "Erro ao criar Usuário"));
            }

            await _userManager.AddToRoleAsync(user, "Cliente");
            return StatusCode(201, ApiResposta<object>.Ok(null!, "Usuário registrado com sucesso!"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !user.Ativo)
                return Unauthorized(ApiResposta<LoginRespostaDto>.Falha("Usuario invalido ou inativo"));

            var passwordIsValid = await _userManager.CheckPasswordAsync(user, dto.Senha);
            if (!passwordIsValid)
                return Unauthorized(ApiResposta<LoginRespostaDto>.Falha("Senha Incorreta"));

            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var token = GerarToken(authClaims);

            var response = new LoginRespostaDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = token.ValidTo,
                NomeUsuario = user.Nome,
                Email = user.Email!,
                FotoPerfilUrl = user.FotoUrl,
                Perfis = roles.ToList()
            };

            return Ok(ApiResposta<LoginRespostaDto>.Ok(response, "Login realizado com sucesso!"));
        }
    }
}
