// ============================================================================
// Arquivo: PacoteService.cs
// Camada: Melobarbershop.Application (Serviços - Implementações)
// Objetivo: Implementar a lógica de negócio para criação, edição e ativação
//           de pacotes promocionais de serviços (combos).
// Papel na Arquitetura:
//   - Gerencia o relacionamento N:N entre Pacote e Servico através da entidade de junção PacoteItem.
//   - Garante que todos os serviços referenciados no DTO existam no banco antes de persistir o pacote.
// ============================================================================

using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

/// <summary>
/// Implementação do serviço de gestão de pacotes de serviços da barbearia.
/// </summary>
public class PacoteService : IPacoteService
{
    private readonly IPacoteRepository _pacoteRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor com injeção dos repositórios de pacotes, serviços e do AutoMapper.
    /// </summary>
    public PacoteService(
        IPacoteRepository pacoteRepository,
        IServicoRepository servicoRepository,
        IMapper mapper)
    {
        _pacoteRepository = pacoteRepository;
        _servicoRepository = servicoRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém um pacote específico com a coleção de serviços incluídos.
    /// </summary>
    public async Task<ApiResposta<PacoteDto>> ObterPorIdAsync(int id)
    {
        try
        {
            var pacote = await _pacoteRepository.ObterPorIdComItensAsync(id);

            if (pacote == null)
                return ApiResposta<PacoteDto>.Falha("Pacote não encontrado");

            var dto = _mapper.Map<PacoteDto>(pacote);
            return ApiResposta<PacoteDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<PacoteDto>.Falha($"Erro ao obter pacote: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna todos os pacotes ativos para comercialização na recepção ou no aplicativo.
    /// </summary>
    public async Task<ApiResposta<IEnumerable<PacoteDto>>> ListarAtivosAsync()
    {
        try
        {
            var pacotes = await _pacoteRepository.ObterAtivosAsync();

            var dto = _mapper.Map<IEnumerable<PacoteDto>>(pacotes);
            return ApiResposta<IEnumerable<PacoteDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<PacoteDto>>.Falha(
                $"Erro ao listar pacotes ativos: {ex.Message}");
        }
    }

    /// <summary>
    /// Retorna todos os pacotes existentes no banco, incluindo os inativos.
    /// </summary>
    public async Task<ApiResposta<IEnumerable<PacoteDto>>> ListarTodosAsync()
    {
        try
        {
            var pacotes = await _pacoteRepository.ObterTodosAsync();

            var dto = _mapper.Map<IEnumerable<PacoteDto>>(pacotes);
            return ApiResposta<IEnumerable<PacoteDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<PacoteDto>>.Falha(
                $"Erro ao listar todos os pacotes: {ex.Message}");
        }
    }

    /// <summary>
    /// Cria um novo pacote promocional validando se cada serviço indicado existe previamente.
    /// </summary>
    public async Task<ApiResposta<PacoteDto>> CriarAsync(CriarPacoteDto criarDto)
    {
        try
        {
            var servicos = new List<Servico>();

            // Validação de integridade: checa se cada ID de serviço é válido
            foreach (var sid in criarDto.ServicoIds)
            {
                var s = await _servicoRepository.ObterPorIdAsync(sid);

                if (s == null)
                    return ApiResposta<PacoteDto>.Falha(
                        $"Serviço {sid} não encontrado");

                servicos.Add(s);
            }

            // Monta a entidade Pacote com os itens de junção PacoteItem
            var pacote = new Pacote
            {
                Nome = criarDto.Nome,
                PrecoTotal = criarDto.PrecoTotal,
                Ativo = true,
                Itens = servicos
                    .Select(s => new PacoteItem
                    {
                        ServicoId = s.Id
                    })
                    .ToList()
            };

            await _pacoteRepository.AdicionarAsync(pacote);

            // Recarrega o pacote com os itens e dados dos serviços para devolver DTO completo
            var salvo = await _pacoteRepository
                .ObterPorIdComItensAsync(pacote.Id);

            var dto = _mapper.Map<PacoteDto>(salvo);
            return ApiResposta<PacoteDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<PacoteDto>.Falha($"Erro ao criar pacote: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza os dados do pacote e redefine a lista de serviços vinculados.
    /// </summary>
    public async Task<ApiResposta<PacoteDto>> AtualizarAsync(
        int id,
        AtualizarPacoteDto atualizarDto)
    {
        try
        {
            var pacote = await _pacoteRepository
                .ObterPorIdComItensAsync(id);

            if (pacote == null)
                return ApiResposta<PacoteDto>.Falha(
                    $"Pacote {id} não encontrado");

            var servicos = new List<Servico>();

            foreach (var sid in atualizarDto.ServicoIds)
            {
                var s = await _servicoRepository.ObterPorIdAsync(sid);

                if (s == null)
                    return ApiResposta<PacoteDto>.Falha(
                        $"Serviço {sid} não encontrado");

                servicos.Add(s);
            }

            pacote.Nome = atualizarDto.Nome;
            pacote.PrecoTotal = atualizarDto.PrecoTotal;
            pacote.Ativo = atualizarDto.Ativo;

            // Substitui a lista de itens da entidade pelo novo conjunto informado
            pacote.Itens = servicos
                .Select(s => new PacoteItem
                {
                    ServicoId = s.Id,
                    PacoteId = pacote.Id
                })
                .ToList();

            await _pacoteRepository.AtualizarAsync(pacote);

            var atualizado = await _pacoteRepository
                .ObterPorIdComItensAsync(pacote.Id);

            var dto = _mapper.Map<PacoteDto>(atualizado);
            return ApiResposta<PacoteDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<PacoteDto>.Falha(
                $"Erro ao atualizar pacote com ID {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Desativa um pacote promocional impedindo novas compras sem perder o histórico do cadastro.
    /// </summary>
    public async Task<ApiResposta<PacoteDto>> DesativarAsync(int id)
    {
        try
        {
            var pacote = await _pacoteRepository.ObterPorIdAsync(id);

            if (pacote == null)
                return ApiResposta<PacoteDto>.Falha(
                    $"Pacote {id} não encontrado");

            pacote.Ativo = false;

            await _pacoteRepository.AtualizarAsync(pacote);

            var atualizado = await _pacoteRepository
                .ObterPorIdComItensAsync(pacote.Id);

            var dto = _mapper.Map<PacoteDto>(atualizado);
            return ApiResposta<PacoteDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<PacoteDto>.Falha(
                $"Erro ao desativar pacote com ID {id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Reativa um pacote previamente inativo.
    /// </summary>
    public async Task<ApiResposta<PacoteDto>> AtivarAsync(int id)
    {
        try
        {
            var pacote = await _pacoteRepository.ObterPorIdAsync(id);

            if (pacote == null)
                return ApiResposta<PacoteDto>.Falha(
                    $"Pacote {id} não encontrado");

            pacote.Ativo = true;

            await _pacoteRepository.AtualizarAsync(pacote);

            var atualizado = await _pacoteRepository
                .ObterPorIdComItensAsync(pacote.Id);

            var dto = _mapper.Map<PacoteDto>(atualizado);
            return ApiResposta<PacoteDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<PacoteDto>.Falha(
                $"Erro ao ativar pacote com ID {id}: {ex.Message}");
        }
    }
}
