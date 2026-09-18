// ============================================================================
// Arquivo: AvaliacaoService.cs
// Camada: Melobarbershop.Application (Serviços - Implementações)
// Objetivo: Implementar a lógica de negócio para registro, controle e consultas
//           estatísticas das avaliações de clientes aos barbeiros.
// Papel na Arquitetura:
//   - Garante as regras de integridade: nota entre 1 e 5 estrelas e apenas uma avaliação por agendamento.
//   - Consolida dados do agendamento para associar automaticamente o BarbeiroId correto.
//   - Utiliza tratamento explícito de exceções de domínio (KeyNotFoundException, InvalidOperationException).
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

/// <summary>
/// Implementação do serviço de gestão de avaliações e satisfação de clientes.
/// </summary>
public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepo;
    private readonly IAgendamentoRepository _agendamentoRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    /// <summary>
    /// Construtor com injeção dos repositórios de avaliação, agendamento e usuário.
    /// </summary>
    public AvaliacaoService(
        IAvaliacaoRepository avaliacaoRepo,
        IAgendamentoRepository agendamentoRepo,
        IUsuarioRepository usuarioRepo)
    {
        _avaliacaoRepo = avaliacaoRepo;
        _agendamentoRepo = agendamentoRepo;
        _usuarioRepo = usuarioRepo;
    }

    /// <summary>
    /// Obtém uma avaliação pelo identificador único e mapeia para DTO.
    /// </summary>
    public async Task<AvaliacaoDto?> ObterPorIdAsync(int id)
    {
        try
        {
            var av = await _avaliacaoRepo.ObterPorIdAsync(id);
            return av == null ? null : MapToDto(av);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter avaliacao com ID {id}.", ex);
        }
    }

    /// <summary>
    /// Obtém a avaliação associada a um determinado agendamento.
    /// </summary>
    public async Task<AvaliacaoDto?> ObterPorAgendamentoAsync(int agendamentoId)
    {
        try
        {
            var av = await _avaliacaoRepo.ObterPorAgendamentoIdAsync(agendamentoId);
            return av == null ? null : MapToDto(av);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter avaliacao para o agendamento {agendamentoId}.", ex);
        }
    }

    /// <summary>
    /// Lista o histórico de avaliações recebidas por um barbeiro.
    /// </summary>
    public async Task<IEnumerable<AvaliacaoDto>> ListarPorBarbeiroAsync(string barbeiroId)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepo.ObterPorBarbeiroAsync(barbeiroId);
            return avaliacoes.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar avaliacoes do barbeiro '{barbeiroId}'.", ex);
        }
    }

    /// <summary>
    /// Lista as avaliações já efetuadas por determinado cliente.
    /// </summary>
    public async Task<IEnumerable<AvaliacaoDto>> ListarPorClienteAsync(string clienteId)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepo.ObterPorClienteAsync(clienteId);
            return avaliacoes.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar avaliacoes do cliente '{clienteId}'.", ex);
        }
    }

    /// <summary>
    /// Monta o sumário analítico de reputação do barbeiro: média de estrelas arredondada a 1 casa decimal e total de feedbacks.
    /// </summary>
    public async Task<ResumoAvaliacoesDto> ObterResumoAvaliacoesBarbeiroAsync(string barbeiroId)
    {
        try
        {
            // Valida existência do profissional
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var media = await _avaliacaoRepo.CalcularMediaAvaliacoesBarbeiroAsync(barbeiroId);
            var avaliacoes = await _avaliacaoRepo.ObterPorBarbeiroAsync(barbeiroId);

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

    /// <summary>
    /// Valida e registra a avaliação garantindo limite de notas (1-5) e unicidade por agendamento.
    /// </summary>
    public async Task<AvaliacaoDto> RegistrarAvaliacaoAsync(CriarAvaliacaoDto dto)
    {
        try
        {
            // Regra de validação: nota de 1 a 5 estrelas
            if (dto.NotaEstrelas < 1 || dto.NotaEstrelas > 5)
                throw new InvalidOperationException("A nota deve ser entre 1 e 5 estrelas.");

            // Valida existência do agendamento
            var agendamento = await _agendamentoRepo.ObterPorIdAsync(dto.AgendamentoId);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento {dto.AgendamentoId} nao encontrado.");

            // Regra de unicidade: impede avaliações duplicadas para o mesmo atendimento
            if (await _avaliacaoRepo.ExisteAvaliacaoParaAgendamentoAsync(dto.AgendamentoId))
                throw new InvalidOperationException("Ja existe uma avaliacao para este agendamento.");

            // Criação da entidade associando o barbeiro diretamente a partir do agendamento auditado
            var avaliacao = new Avaliacao
            {
                AgendamentoId = dto.AgendamentoId,
                ClienteId = dto.ClienteId,
                BarbeiroId = agendamento.BarbeiroId,
                NotaEstrelas = dto.NotaEstrelas,
                Comentario = dto.Comentario,
                DataCriacao = DateTime.UtcNow
            };

            await _avaliacaoRepo.AdicionarAsync(avaliacao);

            // Hidratação dos nomes para retorno completo no DTO
            var cliente = await _usuarioRepo.ObterPorIdAsync(avaliacao.ClienteId);
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(avaliacao.BarbeiroId);

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

    /// <summary>
    /// Método auxiliar privado para projeção direta da entidade Avaliacao para AvaliacaoDto.
    /// </summary>
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

