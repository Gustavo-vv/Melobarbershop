using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IVendaService
{
    Task<ApiResposta<VendaDto>> ObterPorIdAsync(int id);
    Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorClienteAsync(string clienteId);
    Task<ApiResposta<VendaDto>> IniciarVendaAsync(IniciarVendaDto dto);
    Task<ApiResposta<VendaDto>> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto);
    Task<ApiResposta<VendaDto>> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto);
    Task<ApiResposta<VendaDto>> RemoverItemAsync(int vendaId, int vendaItemId);
    Task<ApiResposta<VendaDto>> AplicarDescontoAsync(int vendaId, decimal valorDesconto);
    Task<ApiResposta<VendaDto>> FinalizarVendaAsync(int vendaId);
    Task<ApiResposta<VendaDto>> CancelarVendaAsync(int vendaId, string motivo);
}
