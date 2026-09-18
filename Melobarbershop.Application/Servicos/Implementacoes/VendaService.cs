// ============================================================================
// Arquivo: VendaService.cs
// Camada: Melobarbershop.Application (Serviços - Implementações)
// Objetivo: Implementar a lógica de negócio do fluxo de PDV / Caixa (abertura de comanda,
//           adição de serviços e produtos, aplicação de descontos, baixa de estoque e fechamento).
// Papel na Arquitetura:
//   - Integra Venda com Agendamento, Produto, Servico e Pagamento.
//   - Gerencia comissão e atribuição por barbeiro em cada item da venda.
//   - Garante que itens com produtos abatam o estoque com rastreio de auditoria (TipoMovimentacaoEstoque.SaidaVenda).
//   - Valida liquidação integral da comanda antes de concluir o fechamento.
// ============================================================================

using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

/// <summary>
/// Implementação do serviço de gestão de vendas e operações de caixa / comanda (PDV).
/// </summary>
public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor com injeção de repositórios de vendas, agendamentos, produtos, serviços e do AutoMapper.
    /// </summary>
    public VendaService(
        IVendaRepository vendaRepository,
        IAgendamentoRepository agendamentoRepository,
        IProdutoRepository produtoRepository,
        IServicoRepository servicoRepository,
        IMapper mapper)
    {
        _vendaRepository = vendaRepository;
        _agendamentoRepository = agendamentoRepository;
        _produtoRepository = produtoRepository;
        _servicoRepository = servicoRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém os dados completos de uma venda (itens, serviços, produtos, pagamentos e agendamento vinculado).
    /// </summary>
    public async Task<ApiResposta<VendaDto>> ObterPorIdAsync(int id)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(id);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha("Venda não encontrado.");

            var dto = _mapper.Map<VendaDto>(venda);
            return ApiResposta<VendaDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao obter a venda com ID {ex.Message}.");
        }
    }

    /// <summary>
    /// Lista as vendas realizadas em um intervalo de datas.
    /// </summary>
    public async Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        try
        {
            var vendas = await _vendaRepository.ObterPorPeriodoAsync(inicio, fim);
            var dtos = _mapper.Map<IEnumerable<VendaDto>>(vendas);
            return ApiResposta<IEnumerable<VendaDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<VendaDto>>.Falha($"Erro ao listar vendas por periodo: {ex.Message}");
        }
    }

    /// <summary>
    /// Lista todas as vendas e comandas vinculadas a um cliente específico.
    /// </summary>
    public async Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorClienteAsync(string clienteId)
    {
        try
        {
            var vendas = await _vendaRepository.ObterPorClienteAsync(clienteId);
            var dtos = _mapper.Map<IEnumerable<VendaDto>>(vendas);
            return ApiResposta<IEnumerable<VendaDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<VendaDto>>.Falha($"Erro ao listar vendas do cliente: {ex.Message}.");
        }
    }

    /// <summary>
    /// Inicia uma nova comanda de venda no caixa. Caso originada de um agendamento, importa automaticamente seus serviços e barbeiro.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> IniciarVendaAsync(IniciarVendaDto dto)
    {
        try
        {
            var venda = new Venda
            {
                DataHora = DateTime.UtcNow,
                ClienteId = dto.ClienteId,
                AgendamentoId = dto.AgendamentoId
            };

            // Se associada a agendamento prévio, importa serviços contratados
            if (dto.AgendamentoId.HasValue)
            {
                var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(dto.AgendamentoId.Value);
                if (agendamento == null)
                    return ApiResposta<VendaDto>.Falha($"Agendamento com ID {dto.AgendamentoId} não encontrado.");

                venda.ClienteId ??= agendamento.ClienteId;

                foreach (var item in agendamento.Itens)
                {
                    venda.Itens.Add(new VendaItem
                    {
                        ServicoId = item.ServicoId,
                        BarbeiroId = agendamento.BarbeiroId,
                        Quantidade = 1,
                        PrecoUnitario = item.PrecoCobrado
                    });
                }
            }

            RecalcularTotais(venda);
            await _vendaRepository.AdicionarAsync(venda);

            var vendaCriada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaCriada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao iniciar venda: {ex.Message}");
        }
    }

    /// <summary>
    /// Adiciona um serviço avulso à comanda, vinculando o barbeiro executor e permitindo preço customizado.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            var servico = await _servicoRepository.ObterPorIdAsync(dto.ServicoId);
            if (servico == null)
                return ApiResposta<VendaDto>.Falha($"Serviço com ID {dto.ServicoId} não encontrado.");

            if (!servico.Ativo)
                return ApiResposta<VendaDto>.Falha("Não é possível adicionar um serviço inativo à venda.");

            var preco = dto.PrecoCustomizado ?? servico.Preco;

            venda.Itens.Add(new VendaItem
            {
                VendaId = vendaId,
                ServicoId = servico.Id,
                BarbeiroId = dto.BarbeiroId,
                Quantidade = 1,
                PrecoUnitario = preco
            });

            RecalcularTotais(venda);
            await _vendaRepository.AtualizarAsync(venda);

            var vendaAtualizada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaAtualizada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao adicionar serviço na venda {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Adiciona produto(s) à comanda, validando se há estoque disponível no momento da inclusão.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto)
    {
        if (dto.Quantidade <= 0)
            return ApiResposta<VendaDto>.Falha("A quantidade deve ser maior que zero.");

        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
            if (produto == null)
                return ApiResposta<VendaDto>.Falha($"Produto com ID {dto.ProdutoId} não encontrado.");

            if (!produto.Ativo)
                return ApiResposta<VendaDto>.Falha("Não é possível adicionar um produto inativo à venda.");

            // Verifica soma das quantidades já na comanda + nova quantidade
            var quantidadeJaNaComanda = venda.Itens
                .Where(i => i.ProdutoId == dto.ProdutoId)
                .Sum(i => i.Quantidade);

            if (produto.EstoqueAtual < (quantidadeJaNaComanda + dto.Quantidade))
                return ApiResposta<VendaDto>.Falha($"Estoque insuficiente para o produto '{produto.Nome}'. Disponível: {produto.EstoqueAtual}, solicitado total: {quantidadeJaNaComanda + dto.Quantidade}.");

            var preco = dto.PrecoCustomizado ?? produto.PrecoVenda;

            // Se o mesmo produto já estava na comanda com o mesmo barbeiro e preço, incrementa quantidade
            var itemExistente = venda.Itens.FirstOrDefault(i => i.ProdutoId == dto.ProdutoId && i.BarbeiroId == dto.BarbeiroId && i.PrecoUnitario == preco);
            if (itemExistente != null)
                itemExistente.Quantidade += dto.Quantidade;
            else
                venda.Itens.Add(new VendaItem
                {
                    VendaId = vendaId,
                    ProdutoId = produto.Id,
                    BarbeiroId = dto.BarbeiroId,
                    Quantidade = dto.Quantidade,
                    PrecoUnitario = preco
                });

            RecalcularTotais(venda);
            await _vendaRepository.AtualizarAsync(venda);

            var vendaAtualizada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaAtualizada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao adicionar produto na venda {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Remove um item (serviço ou produto) da comanda e recalcula os subtotais e totais finais.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> RemoverItemAsync(int vendaId, int vendaItemId)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            var item = venda.Itens.FirstOrDefault(i => i.Id == vendaItemId);
            if (item == null)
                return ApiResposta<VendaDto>.Falha($"Item de venda com ID {vendaItemId} não encontrado nesta comanda.");

            venda.Itens.Remove(item);
            RecalcularTotais(venda);
            await _vendaRepository.AtualizarAsync(venda);

            var vendaAtualizada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaAtualizada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao remover item da venda {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Aplica um valor fixo de desconto na comanda e atualiza o total final.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> AplicarDescontoAsync(int vendaId, decimal valorDesconto)
    {
        if (valorDesconto < 0)
            return ApiResposta<VendaDto>.Falha("O valor do desconto não pode ser negativo.");

        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            venda.ValorDesconto = valorDesconto;
            RecalcularTotais(venda);
            await _vendaRepository.AtualizarAsync(venda);

            var vendaAtualizada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaAtualizada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao aplicar desconto na venda {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Finaliza e liquida a venda, validando cobertura total dos pagamentos, dando baixa no estoque de produtos
    /// e atualizando o status do agendamento vinculado para Concluído.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> FinalizarVendaAsync(int vendaId)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            if (!venda.Itens.Any())
                return ApiResposta<VendaDto>.Falha("Não é possível finalizar uma venda sem itens.");

            // Valida se o total pago atinge o total a pagar da comanda
            var totalPago = venda.Pagamentos.Sum(p => p.Valor);
            if (totalPago < venda.ValorFinal)
                return ApiResposta<VendaDto>.Falha($"A venda não está totalmente paga. Valor final: R$ {venda.ValorFinal:F2}, Total pago: R$ {totalPago:F2}. Saldo restante: R$ {(venda.ValorFinal - totalPago):F2}.");

            // Realiza a baixa do estoque e registra a movimentação de auditoria para cada produto vendido
            foreach (var item in venda.Itens.Where(i => i.ProdutoId.HasValue).ToList())
            {
                var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId!.Value);
                if (produto != null)
                {
                    produto.EstoqueAtual -= item.Quantidade;
                    var movimentacao = new MovimentacaoEstoque
                    {
                        ProdutoId = produto.Id,
                        Quantidade = item.Quantidade,
                        Tipo = TipoMovimentacaoEstoque.SaidaVenda,
                        Observacao = $"Saída por venda Nº {venda.Id}",
                        DataHora = DateTime.UtcNow
                    };
                    await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacao);
                    await _produtoRepository.AtualizarAsync(produto);
                }
            }

            // Se originada de agendamento, marca o atendimento como Concluído
            if (venda.AgendamentoId.HasValue)
                await _agendamentoRepository.AtualizarStatusAsync(venda.AgendamentoId.Value, StatusAgendamento.Concluido);

            await _vendaRepository.AtualizarAsync(venda);

            var vendaFinalizada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaFinalizada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao finalizar venda com ID {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Cancela uma venda/comanda aberta caso ainda não possua pagamentos computados.
    /// </summary>
    public async Task<ApiResposta<VendaDto>> CancelarVendaAsync(int vendaId, string motivo)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            // Impede cancelamento de venda que já tem pagamentos confirmados
            if (venda.Pagamentos.Any())
                return ApiResposta<VendaDto>.Falha("Não é possível cancelar uma venda que já possui pagamentos registrados. Estorne os pagamentos antes de cancelar.");

            if (venda.AgendamentoId.HasValue)
                venda.AgendamentoId = null;

            venda.Itens.Clear();
            RecalcularTotais(venda);
            await _vendaRepository.AtualizarAsync(venda);

            var vendaCancelada = await _vendaRepository.ObterPorIdCompletoAsync(venda.Id);
            var vendaDto = _mapper.Map<VendaDto>(vendaCancelada);
            return ApiResposta<VendaDto>.Ok(vendaDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<VendaDto>.Falha($"Erro ao cancelar venda com ID {vendaId}: {ex.Message}");
        }
    }

    /// <summary>
    /// Recalcula o subtotal somando os itens e aplica o desconto fixando o total final nunca abaixo de zero.
    /// </summary>
    private static void RecalcularTotais(Venda venda)
    {
        venda.ValorSubtotal = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        venda.ValorFinal = Math.Max(0, venda.ValorSubtotal - venda.ValorDesconto);
    }
}
