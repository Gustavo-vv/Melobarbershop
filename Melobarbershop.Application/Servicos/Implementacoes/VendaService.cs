using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Interfaces.Services;
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

    public async Task<VendaDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var venda = await _vendaRepository.ObterPorIdCompletoAsync(id, cancellationToken);
        return venda == null ? null : _mapper.Map<VendaDto>(venda);
    }

    public async Task<IEnumerable<VendaDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var vendas = await _vendaRepository.ObterPorPeriodoAsync(inicio, fim, cancellationToken);
        return _mapper.Map<IEnumerable<VendaDto>>(vendas);
    }

    public async Task<IEnumerable<VendaDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        var vendas = await _vendaRepository.ObterPorClienteAsync(clienteId, cancellationToken);
        return _mapper.Map<IEnumerable<VendaDto>>(vendas);
    }

    public async Task<VendaDto> IniciarVendaAsync(IniciarVendaDto dto, CancellationToken cancellationToken = default)
    {
        var venda = new Venda
        {
            DataHora = DateTime.UtcNow,
            ClienteId = dto.ClienteId,
            AgendamentoId = dto.AgendamentoId
        };

        // Se a venda estiver vinculada a um agendamento, importa os serviços e define o cliente automaticamente
        if (dto.AgendamentoId.HasValue)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(dto.AgendamentoId.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Agendamento com ID {dto.AgendamentoId.Value} não encontrado.");

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
        await _vendaRepository.AdicionarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task<VendaDto> AdicionarItemServicoAsync(int vendaId, AdicionarItemServicoDto dto, CancellationToken cancellationToken = default)
    {
        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        var servico = await _servicoRepository.ObterPorIdAsync(dto.ServicoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Serviço com ID {dto.ServicoId} não encontrado.");

        if (!servico.Ativo)
            throw new InvalidOperationException("Não é possível adicionar um serviço inativo à venda.");

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
        await _vendaRepository.AtualizarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task<VendaDto> AdicionarItemProdutoAsync(int vendaId, AdicionarItemProdutoDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(dto.Quantidade));

        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {dto.ProdutoId} não encontrado.");

        if (!produto.Ativo)
            throw new InvalidOperationException("Não é possível adicionar um produto inativo à venda.");

        // Valida se há estoque suficiente considerando o que já possa estar na mesma comanda
        var quantidadeJaNaComanda = venda.Itens
            .Where(i => i.ProdutoId == dto.ProdutoId)
            .Sum(i => i.Quantidade);

        if (produto.EstoqueAtual < (quantidadeJaNaComanda + dto.Quantidade))
            throw new InvalidOperationException($"Estoque insuficiente para o produto '{produto.Nome}'. Disponível: {produto.EstoqueAtual}, solicitado total: {quantidadeJaNaComanda + dto.Quantidade}.");

        var preco = dto.PrecoCustomizado ?? produto.PrecoVenda;

        // Se o produto com mesmo barbeiro já estiver na comanda, apenas incrementa a quantidade
        var itemExistente = venda.Itens.FirstOrDefault(i => i.ProdutoId == dto.ProdutoId && i.BarbeiroId == dto.BarbeiroId && i.PrecoUnitario == preco);
        if (itemExistente != null)
        {
            itemExistente.Quantidade += dto.Quantidade;
        }
        else
        {
            venda.Itens.Add(new VendaItem
            {
                VendaId = vendaId,
                ProdutoId = produto.Id,
                BarbeiroId = dto.BarbeiroId,
                Quantidade = dto.Quantidade,
                PrecoUnitario = preco
            });
        }

        RecalcularTotais(venda);
        await _vendaRepository.AtualizarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task<VendaDto> RemoverItemAsync(int vendaId, int vendaItemId, CancellationToken cancellationToken = default)
    {
        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        var item = venda.Itens.FirstOrDefault(i => i.Id == vendaItemId)
            ?? throw new KeyNotFoundException($"Item de venda com ID {vendaItemId} não encontrado nesta comanda.");

        venda.Itens.Remove(item);
        RecalcularTotais(venda);
        await _vendaRepository.AtualizarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task<VendaDto> AplicarDescontoAsync(int vendaId, decimal valorDesconto, CancellationToken cancellationToken = default)
    {
        if (valorDesconto < 0)
            throw new ArgumentException("O valor do desconto não pode ser negativo.", nameof(valorDesconto));

        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        venda.ValorDesconto = valorDesconto;
        RecalcularTotais(venda);
        await _vendaRepository.AtualizarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task<VendaDto> FinalizarVendaAsync(int vendaId, CancellationToken cancellationToken = default)
    {
        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        if (!venda.Itens.Any())
            throw new InvalidOperationException("Não é possível finalizar uma venda sem itens.");

        var totalPago = venda.Pagamentos.Sum(p => p.Valor);
        if (totalPago < venda.ValorFinal)
            throw new InvalidOperationException($"A venda não está totalmente paga. Valor final: R$ {venda.ValorFinal:F2}, Total pago: R$ {totalPago:F2}. Saldo restante: R$ {(venda.ValorFinal - totalPago):F2}.");

        // 1. Dar baixa no estoque dos produtos vendidos
        var itensProdutos = venda.Itens.Where(i => i.ProdutoId.HasValue).ToList();
        foreach (var item in itensProdutos)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId!.Value, cancellationToken);
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

                await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacao, cancellationToken);
                await _produtoRepository.AtualizarAsync(produto, cancellationToken);
            }
        }

        // 2. Se a venda estiver associada a um agendamento, marca o agendamento como Concluído
        if (venda.AgendamentoId.HasValue)
        {
            await _agendamentoRepository.AtualizarStatusAsync(venda.AgendamentoId.Value, Domain.Enums.StatusAgendamento.Concluido, cancellationToken);
        }

        await _vendaRepository.AtualizarAsync(venda, cancellationToken);

        return (await ObterPorIdAsync(venda.Id, cancellationToken))!;
    }

    public async Task CancelarVendaAsync(int vendaId, string motivo, CancellationToken cancellationToken = default)
    {
        var venda = await _vendaRepository.ObterPorIdCompletoAsync(vendaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Venda com ID {vendaId} não encontrada.");

        if (venda.Pagamentos.Any())
            throw new InvalidOperationException("Não é possível cancelar uma venda que já possui pagamentos registrados. Estorne os pagamentos antes de cancelar.");

        // Se o agendamento estava vinculado, volta ele para confirmado ou mantém o histórico
        if (venda.AgendamentoId.HasValue)
        {
            venda.AgendamentoId = null;
        }

        venda.Itens.Clear();
        RecalcularTotais(venda);

        await _vendaRepository.AtualizarAsync(venda, cancellationToken);
    }

    private static void RecalcularTotais(Venda venda)
    {
        venda.ValorSubtotal = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        venda.ValorFinal = Math.Max(0, venda.ValorSubtotal - venda.ValorDesconto);
    }
}
