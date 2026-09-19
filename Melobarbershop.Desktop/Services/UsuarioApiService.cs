// ============================================================================
// Arquivo: UsuarioApiService.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Serviço cliente para consumo dos endpoints de Usuários (/api/usuarios).
// Papel na Arquitetura:
//   - Obtém a listagem completa de usuários do sistema para as telas de administração do Desktop.
//   - Permite ativar e desativar contas de usuários diretamente pelo aplicativo Desktop.
// ============================================================================

using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

/// <summary>
/// Serviço de integração com a API para consulta e controle de status de usuários.
/// </summary>
public class UsuarioApiService
{
    /// <summary>
    /// Consulta todos os usuários cadastrados no sistema.
    /// </summary>
    public async Task<ApiResposta<List<UsuarioDto>>> ObterTodosAsync()
    {
        return await ApiClient.GetAsync<List<UsuarioDto>>("/api/usuarios");
    }

    /// <summary>
    /// Desativa a conta de um usuário impedindo novos acessos.
    /// </summary>
    public async Task<ApiResposta<bool>> DesativarAsync(string id)
    {
        return await ApiClient.DeleteAsync<bool>($"/api/usuarios/{id}");
    }

    /// <summary>
    /// Reativa a conta de um usuário desativado.
    /// </summary>
    public async Task<ApiResposta<bool>> AtivarAsync(string id)
    {
        return await ApiClient.PutAsync<object, bool>($"/api/usuarios/{id}/ativar", new { });
    }
}
