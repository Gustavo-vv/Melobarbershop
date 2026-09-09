using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IVendaService
{
    Task<VendaDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<VendaDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<VendaDto>> ListarPorClienteAsync(string clienteId);
    Task<VendaDto> IniciarVendaAsync(IniciarVendaDto dto);
    Task<VendaDto> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto);
    Task<VendaDto> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto);
    Task<VendaDto> RemoverItemAsync(int vendaId, int vendaItemId);
    Task<VendaDto> AplicarDescontoAsync(int vendaId, decimal valorDesconto);
    Task<VendaDto> FinalizarVendaAsync(int vendaId);
    Task CancelarVendaAsync(int vendaId, string motivo);
}
