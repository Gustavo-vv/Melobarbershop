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

    public async Task<ServicoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
            return servico == null ? null : _mapper.Map<ServicoDto>(servico);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter servico com ID {id}.", ex);
        }
    }

    public async Task<IEnumerable<ServicoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var servicos = await _servicoRepository.ObterAtivosAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar servicos ativos.", ex);
        }
    }

    public async Task<IEnumerable<ServicoDto>> ListarExibidosNoSiteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var servicos = await _servicoRepository.ObterExibidosNoSiteAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar servicos exibidos no site.", ex);
        }
    }

    public async Task<IEnumerable<ServicoDto>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var servicos = await _servicoRepository.ObterTodosAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ServicoDto>>(servicos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar todos os servicos.", ex);
        }
    }

    public async Task<ServicoDto> CriarAsync(CriarServicoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = _mapper.Map<Servico>(dto);
            await _servicoRepository.AdicionarAsync(servico, cancellationToken);
            return _mapper.Map<ServicoDto>(servico);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao criar servico.", ex);
        }
    }

    public async Task<ServicoDto> AtualizarAsync(int id, AtualizarServicoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
            if (servico == null)
                throw new KeyNotFoundException($"Servico com ID {id} nao encontrado.");

            _mapper.Map(dto, servico);
            await _servicoRepository.AtualizarAsync(servico, cancellationToken);
            return _mapper.Map<ServicoDto>(servico);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao atualizar servico com ID {id}.", ex);
        }
    }

    public async Task DesativarAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
            if (servico == null)
                throw new KeyNotFoundException($"Servico com ID {id} nao encontrado.");

            servico.Ativo = false;
            await _servicoRepository.AtualizarAsync(servico, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao desativar servico com ID {id}.", ex);
        }
    }

    public async Task AtivarAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
            if (servico == null)
                throw new KeyNotFoundException($"Servico com ID {id} nao encontrado.");

            servico.Ativo = true;
            await _servicoRepository.AtualizarAsync(servico, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao ativar servico com ID {id}.", ex);
        }
    }

    public async Task RemoverPermanentementeAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var servico = await _servicoRepository.ObterPorIdAsync(id, cancellationToken);
            if (servico == null)
                throw new KeyNotFoundException($"Servico com ID {id} nao encontrado.");

            await _servicoRepository.RemoverAsync(servico, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Nao e possivel remover este servico permanentemente pois ele possui agendamentos, pacotes ou vendas vinculados. Recomenda-se desativa-lo em vez de excluir permanentemente.", ex);
        }
    }
}
