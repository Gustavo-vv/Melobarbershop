// ============================================================================
// Arquivo: UsuariosController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gerenciamento de contas de usuários (listagem, ativação, desativação e edição).
// Papel na Arquitetura:
//   - Permite à administração listar todos os usuários com seus respectivos perfis (Roles).
//   - Controla o acesso via ativação e bloqueio lógico da conta do usuário.
//   - Permite ao admin editar os dados cadastrais de qualquer usuário (nome, telefone, data nascimento, etc).
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
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
    private readonly IUsuarioService _usuarioService;

    /// <summary>
    /// Construtor com injeção do gerenciador de usuários do ASP.NET Identity e do serviço de domínio.
    /// </summary>
    public UsuariosController(UserManager<ApplicationUser> userManager, IUsuarioService usuarioService)
    {
        _userManager = userManager;
        _usuarioService = usuarioService;
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
                FotoUrl = user.FotoUrl,
                PercentualComissao = user.PercentualComissao,
                DataCadastro = user.DataCadastro,
                Ativo = user.Ativo,
                Roles = roles.ToList()
            });
        }
        return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
    }

    /// <summary>
    /// Atualiza os dados cadastrais de um usuário (nome, telefone, data nascimento, observações, foto, comissão, status).
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(string id, [FromBody] AtualizarUsuarioDto dto)
    {
        try
        {
            var resultado = await _usuarioService.AtualizarAsync(id, dto);
            return Ok(ApiResposta<UsuarioDto>.Ok(resultado, "Usuário atualizado com sucesso!"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResposta<UsuarioDto>.Falha(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResposta<UsuarioDto>.Falha(ex.Message));
        }
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

