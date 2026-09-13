using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.Servicos.Services;

public interface IPagamentoService
{
    Task<PagamentoDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<PagamentoDto>> ListarPorVendaIdAsync(int vendaId);
    Task<PagamentoDto> ProcessarPagamentoAsync(int vendaId, RegistrarPagamentoDto dto);
    Task EstornarPagamentoAsync(int pagamentoId, string motivo);
    Task<decimal> ConsultarTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null);
}
