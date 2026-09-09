using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class ServicoService : IServicoService
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IMapper _mapper;

    public ServicoService(IServicoRepository servicoRepository, IMapper mapper)
    {
        _servicoRepository = servicoRepository;
        _mapper = mapper;
    }

    public async Task<ApiResposta<IEnumerable<ServicoDto>>> ListarAsync(bool incluirInativos = false)
    {
        try
        {
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

    public async Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id);
            if (servico == null)
                return ApiResposta<ServicoDto>.Falha($"Serviço com ID {id} não encontrado.");

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
        catch (Exception ex)
        {
            return ApiResposta<bool>.Falha(
                "Não é possível remover este serviço permanentemente pois ele possui agendamentos, pacotes ou vendas vinculados. Recomenda-se desativá-lo em vez de excluir permanentemente.");
        }
    }
}