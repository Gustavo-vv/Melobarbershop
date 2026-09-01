using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;

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

        [HttpGet("barbeiros")]
        public async Task<IActionResult> ObterBarbeiros()
        {
            var barbeiros = await _userManager.GetUsersInRoleAsync("Barbeiro");
            var dtos = barbeiros.Where(b => b.Ativo).Select(b => new UsuarioDto
            {
                Id = b.Id,
                NomeCompleto = b.NomeCompleto,
                Email = b.Email ?? string.Empty,
                TelefoneWhatsApp = b.TelefoneWhatsApp,
                Especialidade = b.Especialidade,
                FotoPerfilUrl = b.FotoPerfilUrl,
                Ativo = b.Ativo,
                Perfis = new List<string> { "Barbeiro" }
            });

            return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = new List<UsuarioDto>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                dtos.Add(new UsuarioDto
                {
                    Id = u.Id,
                    NomeCompleto = u.NomeCompleto,
                    Email = u.Email ?? string.Empty,
                    TelefoneWhatsApp = u.TelefoneWhatsApp,
                    Especialidade = u.Especialidade,
                    FotoPerfilUrl = u.FotoPerfilUrl,
                    Ativo = u.Ativo,
                    Perfis = roles.ToList()
                });
            }

            return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
        }
    }
}