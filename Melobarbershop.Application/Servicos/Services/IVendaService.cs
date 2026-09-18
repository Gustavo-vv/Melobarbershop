// ============================================================================
// Arquivo: IVendaService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir os contratos de negócio para o módulo de Frente de Caixa (PDV),
//           cobrindo abertura de comanda, inserção/remoção de itens, descontos e encerramento.
// Papel na Arquitetura:
//   - Interface central do fluxo financeiro do estabelecimento.
//   - Orquestra débitos no estoque de produtos quando um item físico é adicionado à venda.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de vendas e comandas de balcão (PDV).
/// </summary>
public interface IVendaService
{
    /// <summary>Obtém uma comanda pelo ID, trazendo itens, produtos, serviços e pagamentos vinculados.</summary>
    Task<ApiResposta<VendaDto>> ObterPorIdAsync(int id);

    /// <summary>Lista todas as vendas e comandas abertas ou fechadas dentro de um intervalo de datas.</summary>
    Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>Lista o histórico de compras e serviços faturados para um cliente específico.</summary>
    Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorClienteAsync(string clienteId);

    /// <summary>Inicia uma nova comanda/venda vazia, opcionalmente associada a um agendamento ou cliente.</summary>
    Task<ApiResposta<VendaDto>> IniciarVendaAsync(IniciarVendaDto dto);

    /// <summary>Adiciona um serviço prestado à comanda, vinculando o barbeiro para cálculo de comissão.</summary>
    Task<ApiResposta<VendaDto>> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto);

    /// <summary>Adiciona um produto à comanda e debita a quantidade correspondente do estoque.</summary>
    Task<ApiResposta<VendaDto>> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto);

    /// <summary>Remove um item da comanda e reverte o débito de estoque se for produto.</summary>
    Task<ApiResposta<VendaDto>> RemoverItemAsync(int vendaId, int vendaItemId);

    /// <summary>Aplica um abatimento financeiro (desconto) no valor subtotal da comanda.</summary>
    Task<ApiResposta<VendaDto>> AplicarDescontoAsync(int vendaId, decimal valorDesconto);

    /// <summary>Finaliza a venda, validando se o valor pago cobre o valor final devido.</summary>
    Task<ApiResposta<VendaDto>> FinalizarVendaAsync(int vendaId);

    /// <summary>Cancela a comanda, estornando movimentações de estoque e registrando o motivo do cancelamento.</summary>
    Task<ApiResposta<VendaDto>> CancelarVendaAsync(int vendaId, string motivo);
}

