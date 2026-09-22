using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class HorarioFuncionamentoService : IHorarioFuncionamentoService
{
    private readonly IHorarioFuncionamentoRepository _horarioRepo;

    private static readonly Dictionary<DayOfWeek, string> NomesDiasPtBr = new()
    {
        { DayOfWeek.Sunday, "Domingo" },
        { DayOfWeek.Monday, "Segunda-feira" },
        { DayOfWeek.Tuesday, "Terça-feira" },
        { DayOfWeek.Wednesday, "Quarta-feira" },
        { DayOfWeek.Thursday, "Quinta-feira" },
        { DayOfWeek.Friday, "Sexta-feira" },
        { DayOfWeek.Saturday, "Sábado" }
    };

    public HorarioFuncionamentoService(IHorarioFuncionamentoRepository horarioRepo)
    {
        _horarioRepo = horarioRepo;
    }

    public async Task<ApiResposta<IEnumerable<HorarioFuncionamentoDto>>> ListarSemanaAsync()
    {
        try
        {
            var horarios = await _horarioRepo.ObterSemanaAsync();

            var dtos = horarios.Select(h => new HorarioFuncionamentoDto
            {
                Id = h.Id,
                DiaSemana = h.DiaSemana,
                NomeDiaSemana = NomesDiasPtBr.TryGetValue(h.DiaSemana, out var nome) ? nome : h.DiaSemana.ToString(),
                Aberto = h.Aberto,
                HoraAbertura = h.HoraAbertura,
                HoraFechamento = h.HoraFechamento
            });

            return ApiResposta<IEnumerable<HorarioFuncionamentoDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<HorarioFuncionamentoDto>>.Falha($"Erro ao listar horários de funcionamento: {ex.Message}");
        }
    }

    public async Task<ApiResposta<HorarioFuncionamentoDto>> AtualizarDiaAsync(DayOfWeek diaSemana, bool aberto, TimeSpan abertura, TimeSpan fechamento)
    {
        try
        {
            var horario = new HorarioFuncionamento
            {
                DiaSemana = diaSemana,
                Aberto = aberto,
                HoraAbertura = abertura,
                HoraFechamento = fechamento
            };

            await _horarioRepo.AdicionarOuAtualizarDiaAsync(horario);

            var atualizado = await _horarioRepo.ObterPorDiaSemanaAsync(diaSemana) ?? horario;

            var dto = new HorarioFuncionamentoDto
            {
                Id = atualizado.Id,
                DiaSemana = atualizado.DiaSemana,
                NomeDiaSemana = NomesDiasPtBr.TryGetValue(atualizado.DiaSemana, out var nome) ? nome : atualizado.DiaSemana.ToString(),
                Aberto = atualizado.Aberto,
                HoraAbertura = atualizado.HoraAbertura,
                HoraFechamento = atualizado.HoraFechamento
            };

            return ApiResposta<HorarioFuncionamentoDto>.Ok(dto, "Horário do dia atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return ApiResposta<HorarioFuncionamentoDto>.Falha($"Erro ao atualizar horário do dia {diaSemana}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<IEnumerable<HorarioEspecialDto>>> ListarEspeciaisAsync(int? ano)
    {
        try
        {
            var especiais = await _horarioRepo.ObterEspeciaisAsync(ano);

            var dtos = especiais.Select(h => new HorarioEspecialDto
            {
                Id = h.Id,
                Data = h.Data,
                Aberto = h.Aberto,
                HoraAbertura = h.HoraAbertura,
                HoraFechamento = h.HoraFechamento,
                Descricao = h.Descricao
            });

            return ApiResposta<IEnumerable<HorarioEspecialDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<HorarioEspecialDto>>.Falha($"Erro ao listar horários especiais: {ex.Message}");
        }
    }

    public async Task<ApiResposta<HorarioEspecialDto>> CriarEspecialAsync(DateTime data, bool aberto, TimeSpan? abertura, TimeSpan? fechamento, string descricao)
    {
        try
        {
            var especial = new HorarioEspecial
            {
                Data = data.Date,
                Aberto = aberto,
                HoraAbertura = aberto ? abertura : null,
                HoraFechamento = aberto ? fechamento : null,
                Descricao = descricao
            };

            await _horarioRepo.AdicionarOuAtualizarEspecialAsync(especial);

            var salvo = await _horarioRepo.ObterEspecialPorDataAsync(data.Date) ?? especial;

            var dto = new HorarioEspecialDto
            {
                Id = salvo.Id,
                Data = salvo.Data,
                Aberto = salvo.Aberto,
                HoraAbertura = salvo.HoraAbertura,
                HoraFechamento = salvo.HoraFechamento,
                Descricao = salvo.Descricao
            };

            return ApiResposta<HorarioEspecialDto>.Ok(dto, "Data especial salva com sucesso.");
        }
        catch (Exception ex)
        {
            return ApiResposta<HorarioEspecialDto>.Falha($"Erro ao salvar horário especial: {ex.Message}");
        }
    }

    public async Task<ApiResposta<bool>> RemoverEspecialAsync(int id)
    {
        try
        {
            var especial = await _horarioRepo.ObterEspecialPorIdAsync(id);
            if (especial == null)
            {
                return ApiResposta<bool>.Falha("Horário especial não encontrado.");
            }

            await _horarioRepo.RemoverEspecialAsync(especial);
            return ApiResposta<bool>.Ok(true, "Horário especial removido com sucesso.");
        }
        catch (Exception ex)
        {
            return ApiResposta<bool>.Falha($"Erro ao remover horário especial: {ex.Message}");
        }
    }
}
