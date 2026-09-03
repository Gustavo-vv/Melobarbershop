using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Interfaces.Services;
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

    public async Task<ServicoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
        return servico == null ? null : _mapper.Map<ServicoDto>(servico);
    }

    public async Task<IEnumerable<ServicoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        var servicos = await _servicoRepository.ObterAtivosAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
    }

    public async Task<IEnumerable<ServicoDto>> ListarExibidosNoSiteAsync(CancellationToken cancellationToken = default)
    {
        var servicos = await _servicoRepository.ObterExibidosNoSiteAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
    }

    public async Task<IEnumerable<ServicoDto>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        var servicos = await _servicoRepository.ObterTodosAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
    }

    public async Task<ServicoDto> CriarAsync(CriarServicoDto dto, CancellationToken cancellationToken = default)
    {
        var servico = _mapper.Map<Servico>(dto);
        await _servicoRepository.AdicionarAsync(servico, cancellationToken);
        return _mapper.Map<ServicoDto>(servico);
    }

    public async Task<ServicoDto> AtualizarAsync(int id, AtualizarServicoDto dto, CancellationToken cancellationToken = default)
    {
        var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Serviço com ID {id} não encontrado.");

        _mapper.Map(dto, servico);
        await _servicoRepository.AtualizarAsync(servico, cancellationToken);

        return _mapper.Map<ServicoDto>(servico);
    }

    public async Task DesativarAsync(int id, CancellationToken cancellationToken = default)
    {
        var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Serviço com ID {id} não encontrado.");

        servico.Ativo = false;
        await _servicoRepository.AtualizarAsync(servico, cancellationToken);
    }

    public async Task AtivarAsync(int id, CancellationToken cancellationToken = default)
    {
        var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Serviço com ID {id} não encontrado.");

        servico.Ativo = true;
        await _servicoRepository.AtualizarAsync(servico, cancellationToken);
    }

    public async Task RemoverPermanentementeAsync(int id, CancellationToken cancellationToken = default)
    {
        var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Serviço com ID {id} não encontrado.");

        try
        {
            await _servicoRepository.RemoverAsync(servico, cancellationToken);
        }
        catch (Exception ex) when (ex.GetType().Name.Contains("DbUpdateException"))
        {
            throw new InvalidOperationException(
                "Não é possível remover este serviço permanentemente pois ele possui agendamentos, pacotes ou vendas vinculados. Recomenda-se desativá-lo em vez de excluir permanentemente.",
                ex);
        }
    }
}
