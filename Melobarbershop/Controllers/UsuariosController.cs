using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Http;
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
        public UsuariosController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                    DataNascimento = user.DataNascimento,
                    PreferenciasNotas = user.PreferenciasNotas,
                    //PercentualComissao = user.PercentualComissao,
                    DataCadastro = user.DataCadastro,
                    Ativo = user.Ativo,
                    Roles = roles.ToList()
                });
            }
            return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
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
