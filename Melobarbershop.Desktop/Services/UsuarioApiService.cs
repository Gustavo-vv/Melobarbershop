using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services
{
    public class UsuarioApiService
    {
        public async Task<ApiResposta<List<UsuarioDto>>> ObterTodosAsync()
        {
            return await ApiClient.GetAsync<List<UsuarioDto>>("/api/usuarios");
        }

        public async Task<ApiResposta<bool>> DesativarAsync(string id)
        {
            return await ApiClient.DeleteAsync<bool>($"/api/usuarios/{id}");
        }

        public async Task<ApiResposta<bool>> AtivarAsync(string id)
        {
            return await ApiClient.PutAsync<object, bool>($"/api/usuarios/{id}/ativar", new { });
        }

        public async Task<ApiResposta<UsuarioDto>> AtualizarAsync(string id, AtualizarDadosClienteDto dto)
        {
            return await ApiClient.PutAsync<AtualizarDadosClienteDto, UsuarioDto>($"/api/usuarios/{id}", dto);
        }
    }
}
