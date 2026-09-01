using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;

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
            var chave = _configuration["Jwt:Chave"] ?? "MelobarbershopChaveSecretaSuperSegura2026MeloVIP!";
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Emissor"],
                audience: _configuration["Jwt:Audiencia"],
                expires: DateTime.UtcNow.AddHours(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest(ApiResposta<object>.Falha("JÃ¡ existe um usuÃ¡rio cadastrado com este e-mail."));

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                NomeCompleto = dto.NomeCompleto,
                TelefoneWhatsApp = dto.TelefoneWhatsApp,
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);
            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResposta<object>.FalhaValidacao(erros, "Erro ao criar conta."));
            }

            await _userManager.AddToRoleAsync(user, "Cliente");
            return StatusCode(201, ApiResposta<object>.Ok(null!, "UsuÃ¡rio registrado com sucesso!"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !user.Ativo)
                return Unauthorized(ApiResposta<LoginRespostaDto>.Falha("E-mail nÃ£o encontrado ou usuÃ¡rio inativo."));

            var senhaValida = await _userManager.CheckPasswordAsync(user, dto.Senha);
            if (!senhaValida)
                return Unauthorized(ApiResposta<LoginRespostaDto>.Falha("Senha incorreta."));

            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.NomeCompleto),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GerarToken(authClaims);

            var resposta = new LoginRespostaDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracao = token.ValidTo,
                Id = user.Id,
                NomeUsuario = user.NomeCompleto,
                Email = user.Email ?? string.Empty,
                FotoPerfilUrl = user.FotoPerfilUrl,
                Perfis = roles.ToList()
            };

            return Ok(ApiResposta<LoginRespostaDto>.Ok(resposta, "Login realizado com sucesso!"));
        }
    }
}