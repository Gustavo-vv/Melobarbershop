using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class VendaService : IVendaService
{
    private readonly IVendaRepository _vendaRepository;
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

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

    public async Task<ApiResposta<IEnumerable<VendaDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        try
        {
            IEnumerable<Venda> vendas;
            vendas = await _vendaRepository.ObterPorPeriodoAsync(inicio, fim);
            var dtos = _mapper.Map<IEnumerable<VendaDto>>(vendas);
            return ApiResposta<IEnumerable<VendaDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<VendaDto>>.Falha($"Erro ao listar vendas por periodo: {ex.Message}");
        }
    }

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

            var quantidadeJaNaComanda = venda.Itens
                .Where(i => i.ProdutoId == dto.ProdutoId)
                .Sum(i => i.Quantidade);

            if (produto.EstoqueAtual < (quantidadeJaNaComanda + dto.Quantidade))
                return ApiResposta<VendaDto>.Falha($"Estoque insuficiente para o produto '{produto.Nome}'. Disponível: {produto.EstoqueAtual}, solicitado total: {quantidadeJaNaComanda + dto.Quantidade}.");

            var preco = dto.PrecoCustomizado ?? produto.PrecoVenda;

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

    public async Task<ApiResposta<VendaDto>> FinalizarVendaAsync(int vendaId)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

            if (!venda.Itens.Any())
                return ApiResposta<VendaDto>.Falha("Não é possível finalizar uma venda sem itens.");

            var totalPago = venda.Pagamentos.Sum(p => p.Valor);
            if (totalPago < venda.ValorFinal)
                return ApiResposta<VendaDto>.Falha($"A venda não está totalmente paga. Valor final: R$ {venda.ValorFinal:F2}, Total pago: R$ {totalPago:F2}. Saldo restante: R$ {(venda.ValorFinal - totalPago):F2}.");

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
                        Tipo = Domain.Enums.TipoMovimentacaoEstoque.SaidaVenda,
                        Observacao = $"Saída por venda Nº {venda.Id}",
                        DataHora = DateTime.UtcNow
                    };
                    await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacao);
                    await _produtoRepository.AtualizarAsync(produto);
                }
            }

            if (venda.AgendamentoId.HasValue)
                await _agendamentoRepository.AtualizarStatusAsync(venda.AgendamentoId.Value, Domain.Enums.StatusAgendamento.Concluido);

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

    public async Task<ApiResposta<VendaDto>> CancelarVendaAsync(int vendaId, string motivo)
    {
        try
        {
            var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId);
            if (venda == null)
                return ApiResposta<VendaDto>.Falha($"Venda com ID {vendaId} não encontrada.");

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

    private static void RecalcularTotais(Venda venda)
    {
        venda.ValorSubtotal = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        venda.ValorFinal = Math.Max(0, venda.ValorSubtotal - venda.ValorDesconto);
    }
}
