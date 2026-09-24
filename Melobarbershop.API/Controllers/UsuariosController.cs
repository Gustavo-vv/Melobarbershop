using System.Security.Claims;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(UserManager<ApplicationUser> userManager, IUsuarioService usuarioService)
        {
            _userManager = userManager;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = new List<UsuarioDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                dtos.Add(new UsuarioDto
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber,
                    DataNascimento = user.DataNascimento,
                    PreferenciasNotas = user.PreferenciasNotas,
                    PercentualComissao = user.PercentualComissao,
                    DataCadastro = user.DataCadastro,
                    Ativo = user.Ativo,
                    Roles = roles.ToList()
                });
            }
            return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> ObterDadosLogado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResposta<UsuarioDto>.Falha("Usuário não identificado no token."));
            }

            var usuario = await _usuarioService.ObterPorIdAsync(userId);
            if (usuario == null)
            {
                return NotFound(ApiResposta<UsuarioDto>.Falha("Usuário não encontrado."));
            }

            return Ok(ApiResposta<UsuarioDto>.Ok(usuario));
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> AtualizarDadosLogado([FromBody] AtualizarDadosClienteDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResposta<UsuarioDto>.Falha("Usuário não identificado no token."));
            }

            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResposta<UsuarioDto>.FalhaValidacao(erros, "Erro na validação dos dados."));
            }

            try
            {
                var usuarioAtualizado = await _usuarioService.AtualizarDadosClienteAsync(userId, dto);
                return Ok(ApiResposta<UsuarioDto>.Ok(usuarioAtualizado, "Dados atualizados com sucesso!"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResposta<UsuarioDto>.Falha(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResposta<UsuarioDto>.Falha(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResposta<UsuarioDto>.Falha($"Erro interno ao atualizar os dados: {ex.Message}"));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(string id, [FromBody] AtualizarDadosClienteDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(ApiResposta<UsuarioDto>.Falha("ID do usuário inválido."));
            }

            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResposta<UsuarioDto>.FalhaValidacao(erros, "Erro na validação dos dados."));
            }

            try
            {
                var usuarioAtualizado = await _usuarioService.AtualizarDadosClienteAsync(id, dto);
                return Ok(ApiResposta<UsuarioDto>.Ok(usuarioAtualizado, "Dados do cliente atualizados com sucesso!"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResposta<UsuarioDto>.Falha(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResposta<UsuarioDto>.Falha(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResposta<UsuarioDto>.Falha($"Erro interno ao atualizar os dados: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest(ApiResposta<bool>.Falha("Usuario não encontrado."));

            user.Ativo = false;
            await _userManager.UpdateAsync(user);

            return Ok(ApiResposta<bool>.Ok(true, "Usuario desativado com sucesso!"));
        }

        [HttpPut("{id}/ativar")]
        public async Task<IActionResult> Ativar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest(ApiResposta<bool>.Falha("Usuario não encontrado."));

            user.Ativo = true;
            await _userManager.UpdateAsync(user);

            return Ok(ApiResposta<bool>.Ok(true, "Usuario ativado com sucesso!"));
        }
    }
}
