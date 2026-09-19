// ============================================================================
// Arquivo: AuthService.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Serviço de autenticação do operador no sistema Desktop.
// Papel na Arquitetura:
//   - Processa o login do operador via API REST e armazena o token na sessão estática de ApiClient.
//   - Permite encerramento de sessão (Logout).
// ============================================================================

using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

/// <summary>
/// Serviço responsável pelo fluxo de autenticação e sessão do usuário no Desktop.
/// </summary>
public class AuthService
{
    /// <summary>
    /// Envia as credenciais de login para a API e inicializa a sessão autenticada em caso de sucesso.
    /// </summary>
    public async Task<ApiResposta<LoginRespostaDto>> LoginAsync(string email, string senha)
    {
        var dto = new LoginDto { Email = email.Trim(), Senha = senha };
        var resposta = await ApiClient.PostAsync<LoginDto, LoginRespostaDto>("/api/auth/login", dto);

        if (resposta.Sucesso && resposta.Dados != null)
        {
            ApiClient.DefinirSessao(resposta.Dados);
        }

        return resposta;
    }

    /// <summary>
    /// Encerra a sessão atual e remove as credenciais do cliente HTTP.
    /// </summary>
    public void Logout()
    {
        ApiClient.EncerrarSessao();
    }
}
