using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
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

    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            return produto == null ? null : _mapper.Map<ProdutoDto>(produto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter produto com ID {id}.", ex);
        }
    }

    public async Task<ProdutoDto?> ObterPorCodigoBarrasAsync(string codigoBarras)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorCodigoBarrasAsync(codigoBarras);
            return produto == null ? null : _mapper.Map<ProdutoDto>(produto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter produto com codigo de barras '{codigoBarras}'.", ex);
        }
    }

    public async Task<IEnumerable<ProdutoDto>> ListarAtivosAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterAtivosAsync();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar produtos ativos.", ex);
        }
    }

    public async Task<IEnumerable<ProdutoDto>> ListarTodosAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar todos os produtos.", ex);
        }
    }

    public async Task<IEnumerable<ProdutoDto>> ListarComEstoqueAbaixoDoMinimoAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterComEstoqueAbaixoDoMinimoAsync();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar produtos com estoque abaixo do minimo.", ex);
        }
    }

    public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
    {
        try
        {
            var existente = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras);
            if (existente != null)
                throw new InvalidOperationException($"Ja existe um produto cadastrado com o codigo de barras '{dto.CodigoBarras}'.");

            var produto = _mapper.Map<Produto>(dto);
            await _produtoRepository.AdicionarAsync(produto);

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
                await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacaoInicial);
            }

            return _mapper.Map<ProdutoDto>(produto);
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao criar produto.", ex);
        }
    }

    public async Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} nao encontrado.");

            if (!string.Equals(produto.CodigoBarras, dto.CodigoBarras, StringComparison.OrdinalIgnoreCase))
            {
                var outroComMesmoCodigo = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras);
                if (outroComMesmoCodigo != null && outroComMesmoCodigo.Id != id)
                    throw new InvalidOperationException($"O codigo de barras '{dto.CodigoBarras}' ja esta em uso por outro produto.");
            }

            _mapper.Map(dto, produto);
            await _produtoRepository.AtualizarAsync(produto);
            return _mapper.Map<ProdutoDto>(produto);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao atualizar produto com ID {id}.", ex);
        }
    }

    public async Task MovimentarEstoqueAsync(MovimentarEstoqueDto dto)
    {
        if (dto.Quantidade <= 0)
            throw new ArgumentException("A quantidade movimentada deve ser maior que zero.", nameof(dto.Quantidade));

        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {dto.ProdutoId} nao encontrado.");

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
                    throw new NotSupportedException($"Tipo de movimentacao '{dto.Tipo}' nao suportado.");
            }

            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                Quantidade = dto.Quantidade,
                Tipo = dto.Tipo,
                Observacao = dto.Observacao,
                DataHora = DateTime.UtcNow
            };

            await _produtoRepository.AdicionarMovimentacaoEstoqueAsync(movimentacao);
            await _produtoRepository.AtualizarAsync(produto);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (NotSupportedException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao movimentar estoque.", ex);
        }
    }

    public async Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null)
    {
        try
        {
            var movimentacoes = await _produtoRepository.ObterMovimentacoesPorProdutoAsync(produtoId, inicio, fim);
            return _mapper.Map<IEnumerable<MovimentacaoEstoqueDto>>(movimentacoes);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar movimentacoes do produto com ID {produtoId}.", ex);
        }
    }

    public async Task<bool> PossuiEstoqueAsync(int produtoId, int quantidade)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);
            if (produto == null || !produto.Ativo)
                return false;
            return produto.EstoqueAtual >= quantidade;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao verificar estoque do produto com ID {produtoId}.", ex);
        }
    }

    public async Task DesativarAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} nao encontrado.");

            produto.Ativo = false;
            await _produtoRepository.AtualizarAsync(produto);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao desativar produto com ID {id}.", ex);
        }
    }

    public async Task AtivarAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} nao encontrado.");

            produto.Ativo = true;
            await _produtoRepository.AtualizarAsync(produto);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao ativar produto com ID {id}.", ex);
        }
    }

    public async Task RemoverPermanentementeAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {id} nao encontrado.");

            await _produtoRepository.RemoverAsync(produto);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Nao e possivel remover este produto permanentemente pois ele possui vendas ou movimentacoes associadas. Recomenda-se desativa-lo em vez de remover permanentemente.", ex);
        }
    }
}
