using System.Net.Http.Headers;

namespace Melobarbershop.UI.Services
{
    public class ApiResposta<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public string[]? Erros { get; set; }
    }

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("MelobarbershopAPI");
            _httpContextAccessor = httpContextAccessor;
        }

        private void ConfigurarTokenDeAutorizacao()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["melobarbershop_token"];

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ApiResposta<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                ConfigurarTokenDeAutorizacao();
                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResposta<T>>();
                    return resultado ?? new ApiResposta<T>
                    {
                        Sucesso = false,
                        Mensagem = "Resposta nula da API."
                    };
                }

                return new ApiResposta<T>
                {
                    Sucesso = false,
                    Mensagem = $"Erro HTTP: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResposta<T>
                {
                    Sucesso = false,
                    Mensagem = $"Erro de conexão: {ex.Message}"
                };
            }
        }

        public async Task<ApiResposta<T>> PostAsync<T, TBody>(string endpoint, TBody corpo)
        {
            try
            {
                ConfigurarTokenDeAutorizacao();
                var response = await _httpClient.PostAsJsonAsync(endpoint, corpo);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResposta<T>>();
                    return resultado ?? new ApiResposta<T> { Sucesso = false, Mensagem = "Resposta nula da API." };
                }

                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro HTTP: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro de conexao: {ex.Message}" };
            }
        }

        public async Task<ApiResposta<T>> PutAsync<T, TBody>(string endpoint, TBody corpo)
        {
            try
            {
                ConfigurarTokenDeAutorizacao();
                var response = await _httpClient.PutAsJsonAsync(endpoint, corpo);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResposta<T>>();
                    return resultado ?? new ApiResposta<T> { Sucesso = false, Mensagem = "Resposta nula da API." };
                }

                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro HTTP: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro de conexao: {ex.Message}" };
            }
        }

        public async Task<ApiResposta<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                ConfigurarTokenDeAutorizacao();
                var response = await _httpClient.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResposta<T>>();
                    return resultado ?? new ApiResposta<T> { Sucesso = false, Mensagem = "Resposta nula da API." };
                }

                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro HTTP: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro de conexao: {ex.Message}" };
            }
        }

        public async Task<ApiResposta<T>> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent conteudo)
        {
            try
            {
                ConfigurarTokenDeAutorizacao();
                var response = await _httpClient.PostAsync(endpoint, conteudo);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResposta<T>>();
                    return resultado ?? new ApiResposta<T> { Sucesso = false, Mensagem = "Resposta nula da API." };
                }

                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro HTTP: {response.StatusCode}" };
            }
            catch (Exception ex)
            {
                return new ApiResposta<T> { Sucesso = false, Mensagem = $"Erro de conexao: {ex.Message}" };
            }
        }
    }
}