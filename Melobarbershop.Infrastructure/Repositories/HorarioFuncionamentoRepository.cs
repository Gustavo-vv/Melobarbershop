using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class HorarioFuncionamentoRepository : IHorarioFuncionamentoRepository
{
    private readonly BarbeariaDbContext _context;

    public HorarioFuncionamentoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HorarioFuncionamento>> ObterSemanaAsync()
    {
        return await _context.HorariosFuncionamento
            .AsNoTracking()
            .OrderBy(h => h.DiaSemana)
            .ToListAsync();
    }

    public async Task<HorarioFuncionamento?> ObterPorDiaSemanaAsync(DayOfWeek diaSemana)
    {
        return await _context.HorariosFuncionamento
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.DiaSemana == diaSemana);
    }

    public async Task AdicionarOuAtualizarDiaAsync(HorarioFuncionamento horario)
    {
        var existente = await _context.HorariosFuncionamento
            .FirstOrDefaultAsync(h => h.DiaSemana == horario.DiaSemana);

        if (existente == null)
        {
            _context.HorariosFuncionamento.Add(horario);
        }
        else
        {
            existente.Aberto = horario.Aberto;
            existente.HoraAbertura = horario.HoraAbertura;
            existente.HoraFechamento = horario.HoraFechamento;
            _context.HorariosFuncionamento.Update(existente);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<HorarioEspecial>> ObterEspeciaisAsync(int? ano = null)
    {
        var query = _context.HorariosEspeciais.AsNoTracking().AsQueryable();

        if (ano.HasValue)
        {
            query = query.Where(h => h.Data.Year == ano.Value);
        }

        return await query.OrderBy(h => h.Data).ToListAsync();
    }

    public async Task<HorarioEspecial?> ObterEspecialPorDataAsync(DateTime data)
    {
        var dataApenas = data.Date;
        return await _context.HorariosEspeciais
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Data == dataApenas);
    }

    public async Task<HorarioEspecial?> ObterEspecialPorIdAsync(int id)
    {
        return await _context.HorariosEspeciais.FindAsync(id);
    }

    public async Task AdicionarOuAtualizarEspecialAsync(HorarioEspecial especial)
    {
        var dataApenas = especial.Data.Date;
        var existente = await _context.HorariosEspeciais
            .FirstOrDefaultAsync(h => h.Data == dataApenas);

        if (existente == null)
        {
            especial.Data = dataApenas;
            _context.HorariosEspeciais.Add(especial);
        }
        else
        {
            existente.Aberto = especial.Aberto;
            existente.HoraAbertura = especial.HoraAbertura;
            existente.HoraFechamento = especial.HoraFechamento;
            existente.Descricao = especial.Descricao;
            _context.HorariosEspeciais.Update(existente);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoverEspecialAsync(HorarioEspecial especial)
    {
        _context.HorariosEspeciais.Remove(especial);
        await _context.SaveChangesAsync();
    }
}
