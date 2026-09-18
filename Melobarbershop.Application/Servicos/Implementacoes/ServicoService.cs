// ============================================================================
// Arquivo: ServicoService.cs
// Camada: Melobarbershop.Application (Serviços - Implementações)
// Objetivo: Implementar a lógica de aplicação e regras de negócio para manutenção
//           do catálogo de serviços da barbearia.
// Papel na Arquitetura:
//   - Faz a ponte entre os controladores da API/Desktop e o repositório IServicoRepository.
//   - Utiliza AutoMapper para conversão entre DTOs e entidades de domínio.
//   - Implementa tratamento de exceções robusto retornando o envelope padrão ApiResposta<T>.
// ============================================================================

using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

/// <summary>
/// Implementação do serviço de gerenciamento do catálogo de serviços.
/// </summary>
public class ServicoService : IServicoService
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor com injeção de dependências do repositório de serviços e do mapeador AutoMapper.
    /// </summary>
    public ServicoService(IServicoRepository servicoRepository, IMapper mapper)
    {
        _servicoRepository = servicoRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Lista os serviços cadastrados. Caso incluirInativos seja false, retorna apenas serviços com Ativo = true.
    /// </summary>
    public async Task<ApiResposta<IEnumerable<ServicoDto>>> ListarAsync(bool incluirInativos = false)
    {
        try
        {
            // Decisão de consulta baseada no filtro: administradores podem ver inativos, clientes só ativos.
            var servicos = incluirInativos
                ? await _servicoRepository.ObterTodosAsync()
                : await _servicoRepository.ObterAtivosAsync();

            var dtos = _mapper.Map<IEnumerable<ServicoDto>>(servicos);
            return ApiResposta<IEnumerable<ServicoDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<ServicoDto>>.Falha($"Erro ao listar serviços: {ex.Message}");
        }
    }

    /// <summary>
    /// Busca um serviço por sua chave primária.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> ObterPorIdAsync(int id)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<ServicoDto>.Falha("Serviço não encontrado.");

            var dto = _mapper.Map<ServicoDto>(servico);
            return ApiResposta<ServicoDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<ServicoDto>.Falha($"Erro ao obter o serviço: {ex.Message}");
        }
    }

    /// <summary>
    /// Mapeia o DTO de entrada para a entidade de domínio, persiste no banco e retorna o DTO criado com seu ID gerado.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> CriarAsync(CriarServicoDto dto)
    {
        try
        {
            var servico = _mapper.Map<Servico>(dto);
            await _servicoRepository.AdicionarAsync(servico);

            var resultado = _mapper.Map<ServicoDto>(servico);
            return ApiResposta<ServicoDto>.Ok(resultado);
        }
        catch (Exception ex)
        {
            return ApiResposta<ServicoDto>.Falha($"Erro ao criar serviço: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza as propriedades de um serviço existente aplicando mapeamento sobre a entidade rastreada.
    /// </summary>
    public async Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<ServicoDto>.Falha($"Serviço com ID {id} não encontrado.");

            // Aplica as alterações do DTO diretamente na instância existente do domínio
            _mapper.Map(dto, servico);
            await _servicoRepository.AtualizarAsync(servico);

            var resultado = _mapper.Map<ServicoDto>(servico);
            return ApiResposta<ServicoDto>.Ok(resultado);
        }
        catch (Exception ex)
        {
            return ApiResposta<ServicoDto>.Falha($"Erro ao atualizar serviço com ID {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Inativa logicamente o serviço (Soft Delete), preservando o histórico de agendamentos e vendas antigas.
    /// </summary>
    public async Task<ApiResposta<bool>> DesativarAsync(int id)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<bool>.Falha($"Serviço com ID {id} não encontrado.");

            servico.Ativo = false;
            await _servicoRepository.AtualizarAsync(servico);

            return ApiResposta<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return ApiResposta<bool>.Falha($"Erro ao desativar serviço com ID {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Reativa um serviço previamente inativado para que volte a ser listado nos agendamentos.
    /// </summary>
    public async Task<ApiResposta<bool>> AtivarAsync(int id)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<bool>.Falha($"Serviço com ID {id} não encontrado.");

            servico.Ativo = true;
            await _servicoRepository.AtualizarAsync(servico);

            return ApiResposta<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return ApiResposta<bool>.Falha($"Erro ao ativar serviço com ID {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Exclui o serviço de forma física/definitiva da base de dados.
    /// Se houver integridade referencial com agendamentos ou itens de venda, o banco disparará exceção tratada no catch.
    /// </summary>
    public async Task<ApiResposta<bool>> RemoverPermanentementeAsync(int id)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<bool>.Falha($"Serviço com ID {id} não encontrado.");

            await _servicoRepository.RemoverAsync(servico);

            return ApiResposta<bool>.Ok(true);
        }
        catch (Exception)
        {
            // Tratamento amigável para violação de Foreign Key do EF Core / SQL Server
            return ApiResposta<bool>.Falha(
                "Não é possível remover este serviço permanentemente pois ele possui agendamentos, pacotes ou vendas vinculados. Recomenda-se desativá-lo em vez de excluir permanentemente.");
        }
    }
}