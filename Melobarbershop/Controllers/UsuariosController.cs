using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Application.DTOs;

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
    }
}
