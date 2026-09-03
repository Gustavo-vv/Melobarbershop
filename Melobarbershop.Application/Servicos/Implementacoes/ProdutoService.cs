using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Interfaces.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;

    public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<ProdutoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id, cancellationToken);
        return produto == null ? null : _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<ProdutoDto?> ObterPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorCodigoBarrasAsync(codigoBarras, cancellationToken);
        return produto == null ? null : _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<IEnumerable<ProdutoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        var produtos = await _produtoRepository.ObterAtivosAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
    }

    public async Task<IEnumerable<ProdutoDto>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        var produtos = await _produtoRepository.ObterTodosAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
    }

    public async Task<IEnumerable<ProdutoDto>> ListarComEstoqueAbaixoDoMinimoAsync(CancellationToken cancellationToken = default)
    {
        var produtos = await _produtoRepository.ObterComEstoqueAbaixoDoMinimoAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
    }

    public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto, CancellationToken cancellationToken = default)
    {
        var existente = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras, cancellationToken);
        if (existente != null)
            throw new InvalidOperationException($"Já existe um produto cadastrado com o código de barras '{dto.CodigoBarras}'.");

        var produto = _mapper.Map<Produto>(dto);
        await _produtoRepository.AdicionarAsync(produto, cancellationToken);

        if (dto.EstoqueInicial > 0)
        {
            var movimentacaoInicial = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                Quantidade = dto.EstoqueInicial,
                Tipo = TipoMovimentacaoEstoque.Entrada,
                Observacao = "Estoque inicial cadastrado",
                DataHora = DateTime.UtcNow
            };
            await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacaoInicial, cancellationToken);
        }

        return _mapper.Map<ProdutoDto>(produto);
    }

    public async Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        if (!string.Equals(produto.CodigoBarras, dto.CodigoBarras, StringComparison.OrdinalIgnoreCase))
        {
            var outroComMesmoCodigo = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras, cancellationToken);
            if (outroComMesmoCodigo != null && outroComMesmoCodigo.Id != id)
                throw new InvalidOperationException($"O código de barras '{dto.CodigoBarras}' já está em uso por outro produto.");
        }

        _mapper.Map(dto, produto);
        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return _mapper.Map<ProdutoDto>(produto);
    }

    public async Task MovimentarEstoqueAsync(MovimentarEstoqueDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Quantidade <= 0)
            throw new ArgumentException("A quantidade movimentada deve ser maior que zero.", nameof(dto.Quantidade));

        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {dto.ProdutoId} não encontrado.");

        switch (dto.Tipo)
        {
            case TipoMovimentacaoEstoque.Entrada:
                produto.EstoqueAtual += dto.Quantidade;
                break;

            case TipoMovimentacaoEstoque.SaidaVenda:
            case TipoMovimentacaoEstoque.UsoInternoBancada:
            case TipoMovimentacaoEstoque.AjustePerda:
                if (produto.EstoqueAtual < dto.Quantidade)
                    throw new InvalidOperationException($"Estoque insuficiente. Estoque atual: {produto.EstoqueAtual}, solicitado: {dto.Quantidade}.");

                produto.EstoqueAtual -= dto.Quantidade;
                break;

            default:
                throw new NotSupportedException($"Tipo de movimentação '{dto.Tipo}' não suportado.");
        }

        var movimentacao = new MovimentacaoEstoque
        {
            ProdutoId = produto.Id,
            Quantidade = dto.Quantidade,
            Tipo = dto.Tipo,
            Observacao = dto.Observacao,
            DataHora = DateTime.UtcNow
        };

        await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacao, cancellationToken);
        await _produtoRepository.AtualizarAsync(produto, cancellationToken);
    }

    public async Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null, CancellationToken cancellationToken = default)
    {
        var movimentacoes = await _produtoRepository.ObterMovimentacoesPorProdutoAsync(produtoId, inicio, fim, cancellationToken);
        return _mapper.Map<IEnumerable<MovimentacaoEstoqueDto>>(movimentacoes);
    }

    public async Task<bool> PossuiEstoqueAsync(int produtoId, int quantidade, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(produtoId, cancellationToken);
        if (produto == null || !produto.Ativo)
            return false;

        return produto.EstoqueAtual >= quantidade;
    }

    public async Task DesativarAsync(int id, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        produto.Ativo = false;
        await _produtoRepository.AtualizarAsync(produto, cancellationToken);
    }

    public async Task AtivarAsync(int id, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        produto.Ativo = true;
        await _produtoRepository.AtualizarAsync(produto, cancellationToken);
    }

    public async Task RemoverPermanentementeAsync(int id, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        try
        {
            await _produtoRepository.RemoverAsync(produto, cancellationToken);
        }
        catch (Exception ex) when (ex.GetType().Name.Contains("DbUpdateException"))
        {
            throw new InvalidOperationException(
                "Não é possível remover este produto permanentemente pois ele possui vendas ou movimentações associadas. Recomenda-se desativá-lo em vez de remover permanentemente.",
                ex);
        }
    }
}
