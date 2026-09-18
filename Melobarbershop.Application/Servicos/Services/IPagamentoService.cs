// ============================================================================
// Arquivo: IPagamentoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir as operações para liquidação financeira, processamento de pagamentos
//           e estornos de comandas no sistema.
// Papel na Arquitetura:
//   - Interface que desacopla os gateways e regras financeiras da camada de apresentação.
//   - Totaliza receitas recebidas por método de pagamento em períodos para relatórios de fechamento de caixa.
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de processamento e conciliação de pagamentos da barbearia.
/// </summary>
public interface IPagamentoService
{
    /// <summary>Obtém um pagamento específico pelo ID.</summary>
    Task<PagamentoDto?> ObterPorIdAsync(int id);

    /// <summary>Lista todos os lançamentos de pagamento associados a uma comanda/venda.</summary>
    Task<IEnumerable<PagamentoDto>> ListarPorVendaIdAsync(int vendaId);

    /// <summary>Processa e registra uma nova entrada de pagamento para uma comanda aberta.</summary>
    Task<PagamentoDto> ProcessarPagamentoAsync(int vendaId, RegistrarPagamentoDto dto);

    /// <summary>Realiza o estorno/cancelamento de um pagamento lançado incorretamente.</summary>
    Task EstornarPagamentoAsync(int pagamentoId, string motivo);

    /// <summary>Calcula o montante total recebido em uma janela de tempo, com filtro opcional por forma de pagamento.</summary>
    Task<decimal> ConsultarTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null);
}

