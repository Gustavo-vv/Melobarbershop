// ============================================================================
// Arquivo: UsuariosController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gerenciamento de contas de usuários (listagem, ativação e desativação).
// Papel na Arquitetura:
//   - Permite à administração listar todos os usuários com seus respectivos perfis (Roles).
//   - Controla o acesso via ativação e bloqueio lógico da conta do usuário.
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pela administração de usuários do sistema.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Construtor com injeção do gerenciador de usuários do ASP.NET Identity.
    /// </summary>
    public UsuariosController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Retorna a listagem de todos os usuários com seus papéis/perfis associados.
    /// </summary>
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
                DataCadastro = user.DataCadastro,
                Ativo = user.Ativo,
                Roles = roles.ToList()
            });
        }
        return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
    }

    /// <summary>
    /// Desativa logicamente a conta de um usuário impedindo novos logins.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return BadRequest(ApiResposta<bool>.Falha("Usuário não encontrado."));

        user.Ativo = false;
        await _userManager.UpdateAsync(user);

        return Ok(ApiResposta<bool>.Ok(true, "Usuário desativado com sucesso!"));
    }

    /// <summary>
    /// Reativa a conta de um usuário que estava suspenso ou desativado.
    /// </summary>
    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> Ativar(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return BadRequest(ApiResposta<bool>.Falha("Usuário não encontrado."));

        user.Ativo = true;
        await _userManager.UpdateAsync(user);

        return Ok(ApiResposta<bool>.Ok(true, "Usuário ativado com sucesso!"));
    }
}
