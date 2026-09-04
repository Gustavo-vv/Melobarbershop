using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.Servicos.Services;

public interface IPagamentoService
{
    Task<PagamentoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PagamentoDto>> ListarPorVendaIdAsync(int vendaId, CancellationToken cancellationToken = default);
    Task<PagamentoDto> ProcessarPagamentoAsync(int vendaId, RegistrarPagamentoDto dto, CancellationToken cancellationToken = default);
    Task EstornarPagamentoAsync(int pagamentoId, string motivo, CancellationToken cancellationToken = default);
    Task<decimal> ConsultarTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null, CancellationToken cancellationToken = default);
}
