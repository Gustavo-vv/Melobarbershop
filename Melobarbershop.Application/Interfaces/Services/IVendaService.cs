using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Interfaces.Services;

public interface IVendaService
{
    Task<VendaDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VendaDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<VendaDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<VendaDto> IniciarVendaAsync(IniciarVendaDto dto, CancellationToken cancellationToken = default);
    Task<VendaDto> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto, CancellationToken cancellationToken = default);
    Task<VendaDto> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto, CancellationToken cancellationToken = default);
    Task<VendaDto> RemoverItemAsync(int vendaId, int vendaItemId, CancellationToken cancellationToken = default);
    Task<VendaDto> AplicarDescontoAsync(int vendaId, decimal valorDesconto, CancellationToken cancellationToken = default);
    Task<VendaDto> FinalizarVendaAsync(int vendaId, CancellationToken cancellationToken = default);
    Task CancelarVendaAsync(int vendaId, string motivo, CancellationToken cancellationToken = default);
}
