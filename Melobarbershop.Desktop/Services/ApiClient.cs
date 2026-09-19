// ============================================================================
// Arquivo: ApiClient.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Cliente HTTP centralizado e estático para consumo da API REST pelo aplicativo Windows Forms.
// Papel na Arquitetura:
//   - Gerencia a sessão de autenticação do operador (Token JWT e cabeçalhos Bearer).
//   - Encapsula métodos genéricos GetAsync, PostAsync, PutAsync, DeleteAsync e PatchAsync.
//   - Trata serialização e desserialização JSON resiliente com suporte a envelopes ApiResposta<T>.
// ============================================================================

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Melobarbershop.Desktop.Configuration;
using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

/// <summary>
/// Cliente HTTP estático responsável por toda a comunicação do Desktop com a API REST.
/// </summary>
public static class ApiClient
{
    private static readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions;

    /// <summary>Token JWT armazenado da sessão atual do usuário logado.</summary>
    public static string? Token { get; private set; }

    /// <summary>Informações de perfil e permissões do usuário logado no aplicativo Desktop.</summary>
    public static LoginRespostaDto? UsuarioLogado { get; private set; }

    static ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(AppConfig.ApiBaseUrl),
            Timeout = TimeSpan.FromSeconds(15)
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Recarrega o endereço base da API a partir das configurações dinâmicas de AppConfig.
    /// </summary>
    public static void AtualizarBaseAddress()
    {
        _httpClient.BaseAddress = new Uri(AppConfig.ApiBaseUrl);
    }

    /// <summary>
    /// Define a sessão autenticada injetando o cabeçalho Authorization: Bearer em todas as requisições subsequentes.
    /// </summary>
    public static void DefinirSessao(LoginRespostaDto loginInfo)
    {
        UsuarioLogado = loginInfo;
        Token = loginInfo.Token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
    }

    /// <summary>
    /// Limpa o token e encerra a sessão ativa do usuário no aplicativo Desktop.
    /// </summary>
    public static void EncerrarSessao()
    {
        UsuarioLogado = null;
        Token = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    /// <summary>Indica se há uma sessão autenticada com token válido configurado no cliente HTTP.</summary>
    public static bool EstaAutenticado => !string.IsNullOrEmpty(Token);

    /// <summary>
    /// Executa uma requisição HTTP GET para o endpoint informado e desserializa o envelope de resposta.
    /// </summary>
    public static async Task<ApiResposta<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResposta<T>>(content, _jsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch
                {
                    // Fallback se o corpo da resposta for o tipo direto T
                    var directParsed = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                    return new ApiResposta<T>
                    {
                        Sucesso = response.IsSuccessStatusCode,
                        Dados = directParsed,
                        Mensagem = response.IsSuccessStatusCode ? "Sucesso" : "Resposta recebida"
                    };
                }
            }

            return new ApiResposta<T>
            {
                Sucesso = response.IsSuccessStatusCode,
                Mensagem = response.ReasonPhrase ?? "Falha na requisição"
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = $"Falha ao conectar com a API: {ex.Message}. Verifique se a API está em execução em {AppConfig.ApiBaseUrl}."
            };
        }
        catch (TaskCanceledException)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = "Tempo limite da requisição esgotado (Timeout). A API demorou para responder."
            };
        }
        catch (Exception ex)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = $"Erro inesperado: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Executa uma requisição HTTP POST enviando um payload serializado em JSON.
    /// </summary>
    public static async Task<ApiResposta<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(responseString))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResposta<TResponse>>(responseString, _jsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch
                {
                    var directParsed = JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                    return new ApiResposta<TResponse>
                    {
                        Sucesso = response.IsSuccessStatusCode,
                        Dados = directParsed,
                        Mensagem = response.IsSuccessStatusCode ? "Sucesso" : "Resposta recebida"
                    };
                }
            }

            return new ApiResposta<TResponse>
            {
                Sucesso = response.IsSuccessStatusCode,
                Mensagem = response.ReasonPhrase ?? "Falha no envio"
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiResposta<TResponse>
            {
                Sucesso = false,
                Mensagem = $"Falha ao conectar com a API: {ex.Message}."
            };
        }
        catch (Exception ex)
        {
            return new ApiResposta<TResponse>
            {
                Sucesso = false,
                Mensagem = $"Erro inesperado: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Executa uma requisição HTTP PUT para atualização completa de recursos na API.
    /// </summary>
    public static async Task<ApiResposta<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(responseString))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResposta<TResponse>>(responseString, _jsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch
                {
                    var directParsed = JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                    return new ApiResposta<TResponse>
                    {
                        Sucesso = response.IsSuccessStatusCode,
                        Dados = directParsed,
                        Mensagem = response.IsSuccessStatusCode ? "Sucesso" : "Resposta recebida"
                    };
                }
            }

            return new ApiResposta<TResponse>
            {
                Sucesso = response.IsSuccessStatusCode,
                Mensagem = response.ReasonPhrase ?? "Falha na atualização"
            };
        }
        catch (Exception ex)
        {
            return new ApiResposta<TResponse>
            {
                Sucesso = false,
                Mensagem = $"Erro: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Executa uma requisição HTTP DELETE para exclusão ou inativação de recursos na API.
    /// </summary>
    public static async Task<ApiResposta<TResponse>> DeleteAsync<TResponse>(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(responseString))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResposta<TResponse>>(responseString, _jsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch
                {
                    var directParsed = JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                    return new ApiResposta<TResponse>
                    {
                        Sucesso = response.IsSuccessStatusCode,
                        Dados = directParsed,
                        Mensagem = response.IsSuccessStatusCode ? "Sucesso" : "Resposta recebida"
                    };
                }
            }

            return new ApiResposta<TResponse>
            {
                Sucesso = response.IsSuccessStatusCode,
                Mensagem = response.ReasonPhrase ?? "Falha na exclusão"
            };
        }
        catch (Exception ex)
        {
            return new ApiResposta<TResponse>
            {
                Sucesso = false,
                Mensagem = $"Erro: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Executa uma requisição HTTP PATCH para atualizações parciais ou transições de status.
    /// </summary>
    public static async Task<ApiResposta<TResponse>> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(responseString))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<ApiResposta<TResponse>>(responseString, _jsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch
                {
                    var directParsed = JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                    return new ApiResposta<TResponse>
                    {
                        Sucesso = response.IsSuccessStatusCode,
                        Dados = directParsed,
                        Mensagem = response.IsSuccessStatusCode ? "Sucesso" : "Resposta recebida"
                    };
                }
            }

            return new ApiResposta<TResponse>
            {
                Sucesso = response.IsSuccessStatusCode,
                Mensagem = response.ReasonPhrase ?? "Falha na operação"
            };
        }
        catch (Exception ex)
        {
            return new ApiResposta<TResponse>
            {
                Sucesso = false,
                Mensagem = $"Erro: {ex.Message}"
            };
        }
    }
}
