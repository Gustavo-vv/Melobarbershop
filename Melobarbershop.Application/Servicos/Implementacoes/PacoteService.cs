using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class PacoteService : IPacoteService
{
    private readonly IPacoteRepository _pacoteRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

    public PacoteService(
        IPacoteRepository pacoteRepository,
        IServicoRepository servicoRepository,
        IMapper mapper)
    {
        _pacoteRepository = pacoteRepository;
        _servicoRepository = servicoRepository;
        _mapper = mapper;
    }

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

    public async Task<ApiResposta<PacoteDto>> CriarAsync(CriarPacoteDto criarDto)
    {
        try
        {
            var servicos = new List<Servico>();

            foreach (var sid in criarDto.ServicoIds)
            {
                var s = await _servicoRepository.ObterPorIdAsync(sid);

                if (s == null)
                    return ApiResposta<PacoteDto>.Falha(
                        $"Serviço {sid} não encontrado");

                servicos.Add(s);
            }

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