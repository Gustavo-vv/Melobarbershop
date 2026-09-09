using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IAvaliacaoService
{
    Task<AvaliacaoDto?> ObterPorIdAsync(int id);
    Task<AvaliacaoDto?> ObterPorAgendamentoAsync(int agendamentoId);
    Task<IEnumerable<AvaliacaoDto>> ListarPorBarbeiroAsync(string barbeiroId);
    Task<IEnumerable<AvaliacaoDto>> ListarPorClienteAsync(string clienteId);
    Task<ResumoAvaliacoesDto> ObterResumoAvaliacoesBarbeiroAsync(string barbeiroId);
    Task<AvaliacaoDto> RegistrarAvaliacaoAsync(CriarAvaliacaoDto dto);
}
