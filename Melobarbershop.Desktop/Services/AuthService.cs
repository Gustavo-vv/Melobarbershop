using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services
{
    public class AuthService
    {
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

        public void Logout()
        {
            ApiClient.EncerrarSessao();
        }
    }
}
