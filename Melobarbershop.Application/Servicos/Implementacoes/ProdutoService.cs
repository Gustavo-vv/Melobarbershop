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

    public async Task<ApiResposta<ProdutoDto>> ObterPorIdAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha("Produto não encontrado.");

            var dto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao obter produto com ID {ex.Message}.");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> ObterPorCodigoBarrasAsync(string codigoBarras)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorCodigoBarrasAsync(codigoBarras);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com código de barras: {codigoBarras} não encontrado.");

            var dto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao obter produto com codigo de barras {ex.Message}.");
        }
    }

    public async Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarAtivosAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterAtivosAsync();
            var dto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return ApiResposta<IEnumerable<ProdutoDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<ProdutoDto>>.Falha($"Erro ao listar produtos ativos {ex.Message}.");
        }
    }

    public async Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarTodosAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterTodosAsync();
            var dto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return ApiResposta<IEnumerable<ProdutoDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<ProdutoDto>>.Falha($"Erro ao listar todos os produtos: {ex.Message}");
        }
    }

    public async Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarComEstoqueAbaixoDoMinimoAsync()
    {
        try
        {
            var produtos = await _produtoRepository.ObterComEstoqueAbaixoDoMinimoAsync();
            var dto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return ApiResposta<IEnumerable<ProdutoDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<ProdutoDto>>.Falha($"Erro ao listar produtos com estoque abaixo do mínimo: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> CriarAsync(CriarProdutoDto dto)
    {
        try
        {
            var existente = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras);
            if (existente != null)
                return ApiResposta<ProdutoDto>.Falha($"Já existe um produto cadastrado com o código de barras '{dto.CodigoBarras}'.");

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

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao criar produto: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com ID {id} não encontrado.");

            if (!string.Equals(produto.CodigoBarras, dto.CodigoBarras, StringComparison.OrdinalIgnoreCase))
            {
                var outroComMesmoCodigo = await _produtoRepository.ObterPorCodigoBarrasAsync(dto.CodigoBarras);
                if (outroComMesmoCodigo != null && outroComMesmoCodigo.Id != id)
                    return ApiResposta<ProdutoDto>.Falha($"O código de barras '{dto.CodigoBarras}' já está em uso por outro produto.");
            }

            _mapper.Map(dto, produto);
            await _produtoRepository.AtualizarAsync(produto);

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao atualizar produto com ID {id}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> MovimentarEstoqueAsync(MovimentarEstoqueDto dto)
    {
        if (dto.Quantidade <= 0)
            return ApiResposta<ProdutoDto>.Falha("A quantidade movimentada deve ser maior que zero.");

        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com ID {dto.ProdutoId} não encontrado.");

            switch (dto.Tipo)
            {
                case TipoMovimentacaoEstoque.Entrada:
                    produto.EstoqueAtual += dto.Quantidade;
                    break;

                case TipoMovimentacaoEstoque.SaidaVenda:
                case TipoMovimentacaoEstoque.UsoInternoBancada:
                case TipoMovimentacaoEstoque.AjustePerda:
                    if (produto.EstoqueAtual < dto.Quantidade)
                        return ApiResposta<ProdutoDto>.Falha($"Estoque insuficiente. Estoque atual: {produto.EstoqueAtual}, solicitado: {dto.Quantidade}.");
                    produto.EstoqueAtual -= dto.Quantidade;
                    break;

                default:
                    return ApiResposta<ProdutoDto>.Falha($"Tipo de movimentação '{dto.Tipo}' não suportado.");
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

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao movimentar estoque: {ex.Message}");
        }
    }

    public async Task<ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null)
    {
        try
        {
            var movimentacoes = await _produtoRepository.ObterMovimentacoesPorProdutoAsync(produtoId, inicio, fim);
            var dto = _mapper.Map<IEnumerable<MovimentacaoEstoqueDto>>(movimentacoes);
            return ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>.Falha($"Erro ao listar movimentações do produto com ID {produtoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<bool>> PossuiEstoqueAsync(int produtoId, int quantidade)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);
            if (produto == null || !produto.Ativo)
                return ApiResposta<bool>.Ok(false);

            return ApiResposta<bool>.Ok(produto.EstoqueAtual >= quantidade);
        }
        catch (Exception ex)
        {
            return ApiResposta<bool>.Falha($"Erro ao verificar estoque do produto com ID {produtoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> DesativarAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com ID {id} não encontrado.");

            produto.Ativo = false;
            await _produtoRepository.AtualizarAsync(produto);

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao desativar produto com ID {id}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> AtivarAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com ID {id} não encontrado.");

            produto.Ativo = true;
            await _produtoRepository.AtualizarAsync(produto);

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha($"Erro ao ativar produto com ID {id}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<ProdutoDto>> RemoverPermanentementeAsync(int id)
    {
        try
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto == null)
                return ApiResposta<ProdutoDto>.Falha($"Produto com ID {id} não encontrado.");

            await _produtoRepository.RemoverAsync(produto);

            var produtoDto = _mapper.Map<ProdutoDto>(produto);
            return ApiResposta<ProdutoDto>.Ok(produtoDto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ProdutoDto>.Falha(
                $"Não é possível remover este produto permanentemente pois ele possui vendas ou movimentações associadas. Recomenda-se desativá-lo em vez de remover permanentemente. ({ex.Message})");
        }
    }
}
