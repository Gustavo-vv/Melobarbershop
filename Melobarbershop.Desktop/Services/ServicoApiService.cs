// ============================================================================
// Arquivo: ServicoApiService.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Serviço cliente para consumo dos endpoints de Serviços (/api/servicos).
// Papel na Arquitetura:
//   - Encapsula as chamadas de CRUD de serviços para os formulários do Desktop (ex: UcServicos).
//   - Trata rotinas de ativação, desativação e exclusão permanente.
// ============================================================================

using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

/// <summary>
/// Serviço responsável pela integração com a API para operações de Serviços.
/// </summary>
public class ServicoApiService
{
    /// <summary>
    /// Consulta todos os serviços cadastrados, incluindo os desativados.
    /// </summary>
    public async Task<ApiResposta<List<ServicoDto>>> ObterTodosAsync()
    {
        return await ApiClient.GetAsync<List<ServicoDto>>("/api/servicos/todos");
    }

    /// <summary>
    /// Consulta apenas os serviços com status ativo.
    /// </summary>
    public async Task<ApiResposta<List<ServicoDto>>> ObterAtivosAsync()
    {
        return await ApiClient.GetAsync<List<ServicoDto>>("/api/servicos");
    }

    /// <summary>
    /// Obtém os dados detalhados de um serviço pelo seu ID.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> ObterPorIdAsync(int id)
    {
        return await ApiClient.GetAsync<ServicoDto>($"/api/servicos/id?id={id}");
    }

    /// <summary>
    /// Envia requisição para cadastrar um novo serviço na barbearia.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> CadastrarAsync(CriarServicoDto dto)
    {
        return await ApiClient.PostAsync<CriarServicoDto, ServicoDto>("/api/servicos", dto);
    }

    /// <summary>
    /// Envia requisição para atualizar os dados de um serviço existente.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto)
    {
        return await ApiClient.PutAsync<AtualizarServicoDto, ServicoDto>($"/api/servicos/id?id={id}", dto);
    }

    /// <summary>
    /// Desativa logicamente um serviço para que não possa ser mais agendado.
    /// </summary>
    public async Task<ApiResposta<bool>> DesativarAsync(int id)
    {
        return await ApiClient.DeleteAsync<bool>($"/api/servicos/{id}/desativar");
    }

    /// <summary>
    /// Reativa um serviço previamente desativado.
    /// </summary>
    public async Task<ApiResposta<bool>> ReativarAsync(int id)
    {
        return await ApiClient.PutAsync<object, bool>($"/api/servicos/{id}/reativar", new { });
    }

    /// <summary>
    /// Exclui permanentemente um serviço do banco de dados (exclusão física).
    /// </summary>
    public async Task<ApiResposta<bool>> ExcluirPermanenteAsync(int id)
    {
        return await ApiClient.DeleteAsync<bool>($"/api/servicos/{id}/permanente");
    }
}
