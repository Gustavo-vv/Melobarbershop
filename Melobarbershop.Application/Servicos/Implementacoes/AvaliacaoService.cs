using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepo;
    private readonly IAgendamentoRepository _agendamentoRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    public AvaliacaoService(
        IAvaliacaoRepository avaliacaoRepo,
        IAgendamentoRepository agendamentoRepo,
        IUsuarioRepository usuarioRepo)
    {
        _avaliacaoRepo = avaliacaoRepo;
        _agendamentoRepo = agendamentoRepo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<AvaliacaoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var av = await _avaliacaoRepo.ObterPorIdAsync(id, cancellationToken);
            return av == null ? null : MapToDto(av);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter avaliacao com ID {id}.", ex);
        }
    }

    public async Task<AvaliacaoDto?> ObterPorAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            var av = await _avaliacaoRepo.ObterPorAgendamentoIdAsync(agendamentoId, cancellationToken);
            return av == null ? null : MapToDto(av);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter avaliacao para o agendamento {agendamentoId}.", ex);
        }
    }

    public async Task<IEnumerable<AvaliacaoDto>> ListarPorBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepo.ObterPorBarbeiroAsync(barbeiroId, cancellationToken);
            return avaliacoes.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar avaliacoes do barbeiro '{barbeiroId}'.", ex);
        }
    }

    public async Task<IEnumerable<AvaliacaoDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepo.ObterPorClienteAsync(clienteId, cancellationToken);
            return avaliacoes.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar avaliacoes do cliente '{clienteId}'.", ex);
        }
    }

    public async Task<ResumoAvaliacoesDto> ObterResumoAvaliacoesBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var media = await _avaliacaoRepo.CalcularMediaAvaliacoesBarbeiroAsync(barbeiroId, cancellationToken);
            var avaliacoes = await _avaliacaoRepo.ObterPorBarbeiroAsync(barbeiroId, cancellationToken);

            return new ResumoAvaliacoesDto
            {
                BarbeiroId = barbeiroId,
                NomeBarbeiro = barbeiro.Nome,
                MediaEstrelas = Math.Round(media, 1),
                TotalAvaliacoes = avaliacoes.Count()
            };
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter resumo de avaliacoes do barbeiro '{barbeiroId}'.", ex);
        }
    }

    public async Task<AvaliacaoDto> RegistrarAvaliacaoAsync(CriarAvaliacaoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (dto.NotaEstrelas < 1 || dto.NotaEstrelas > 5)
                throw new InvalidOperationException("A nota deve ser entre 1 e 5 estrelas.");

            var agendamento = await _agendamentoRepo.ObterPorIdAsync(dto.AgendamentoId, cancellationToken);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento {dto.AgendamentoId} nao encontrado.");

            if (await _avaliacaoRepo.ExisteAvaliacaoParaAgendamentoAsync(dto.AgendamentoId, cancellationToken))
                throw new InvalidOperationException("Ja existe uma avaliacao para este agendamento.");

            var avaliacao = new Avaliacao
            {
                AgendamentoId = dto.AgendamentoId,
                ClienteId = dto.ClienteId,
                BarbeiroId = agendamento.BarbeiroId,
                NotaEstrelas = dto.NotaEstrelas,
                Comentario = dto.Comentario,
                DataCriacao = DateTime.UtcNow
            };

            await _avaliacaoRepo.AdicionarAsync(avaliacao, cancellationToken);

            var salva = await _avaliacaoRepo.ObterPorIdAsync(avaliacao.Id, cancellationToken);
            var cliente = await _usuarioRepo.ObterPorIdAsync(avaliacao.ClienteId, cancellationToken);
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(avaliacao.BarbeiroId, cancellationToken);

            return new AvaliacaoDto
            {
                Id = avaliacao.Id,
                AgendamentoId = avaliacao.AgendamentoId,
                ClienteId = avaliacao.ClienteId,
                NomeCliente = cliente?.Nome ?? string.Empty,
                BarbeiroId = avaliacao.BarbeiroId,
                NomeBarbeiro = barbeiro?.Nome ?? string.Empty,
                NotaEstrelas = avaliacao.NotaEstrelas,
                Comentario = avaliacao.Comentario,
                DataCriacao = avaliacao.DataCriacao
            };
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao registrar avaliacao.", ex);
        }
    }

    private static AvaliacaoDto MapToDto(Avaliacao av) => new()
    {
        Id = av.Id,
        AgendamentoId = av.AgendamentoId,
        ClienteId = av.ClienteId,
        NomeCliente = av.Cliente?.Nome ?? string.Empty,
        BarbeiroId = av.BarbeiroId,
        NomeBarbeiro = av.Barbeiro?.Nome ?? string.Empty,
        NotaEstrelas = av.NotaEstrelas,
        Comentario = av.Comentario,
        DataCriacao = av.DataCriacao
    };
}
