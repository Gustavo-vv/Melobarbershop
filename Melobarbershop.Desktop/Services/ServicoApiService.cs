using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services
{
    public class ServicoApiService
    {
        public async Task<ApiResposta<List<ServicoDto>>> ObterTodosAsync()
        {
            return await ApiClient.GetAsync<List<ServicoDto>>("/api/servicos/todos");
        }

        public async Task<ApiResposta<List<ServicoDto>>> ObterAtivosAsync()
        {
            return await ApiClient.GetAsync<List<ServicoDto>>("/api/servicos");
        }

        public async Task<ApiResposta<ServicoDto>> ObterPorIdAsync(int id)
        {
            return await ApiClient.GetAsync<ServicoDto>($"/api/servicos/id?id={id}");
        }

        public async Task<ApiResposta<ServicoDto>> CadastrarAsync(CriarServicoDto dto)
        {
            return await ApiClient.PostAsync<CriarServicoDto, ServicoDto>("/api/servicos", dto);
        }

        public async Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto)
        {
            return await ApiClient.PutAsync<AtualizarServicoDto, ServicoDto>($"/api/servicos/id?id={id}", dto);
        }

        public async Task<ApiResposta<bool>> DesativarAsync(int id)
        {
            return await ApiClient.DeleteAsync<bool>($"/api/servicos/{id}/desativar");
        }

        public async Task<ApiResposta<bool>> ReativarAsync(int id)
        {
            return await ApiClient.PutAsync<object, bool>($"/api/servicos/{id}/reativar", new { });
        }

        public async Task<ApiResposta<bool>> ExcluirPermanenteAsync(int id)
        {
            return await ApiClient.DeleteAsync<bool>($"/api/servicos/{id}/permanente");
        }
    }
}
