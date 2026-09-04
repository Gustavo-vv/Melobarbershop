using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class PacoteService : IPacoteService
{
    private readonly IPacoteRepository _pacoteRepo;
    private readonly IServicoRepository _servicoRepo;
    private readonly IMapper _mapper;

    public PacoteService(IPacoteRepository pacoteRepo, IServicoRepository servicoRepo, IMapper mapper)
    {
        _pacoteRepo = pacoteRepo;
        _servicoRepo = servicoRepo;
        _mapper = mapper;
    }

    public async Task<PacoteDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pacote = await _pacoteRepo.ObterPorIdComItensAsync(id, cancellationToken);
        if (pacote == null) return null;
        return MapToDto(pacote);
    }

    public async Task<IEnumerable<PacoteDto>> ListarAtivosAsync(CancellationToken cancellationToken = default)
    {
        var pacotes = await _pacoteRepo.ObterAtivosAsync(cancellationToken);
        return pacotes.Select(MapToDto);
    }

    public async Task<IEnumerable<PacoteDto>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        var pacotes = await _pacoteRepo.ObterTodosAsync(cancellationToken);
        return pacotes.Select(MapToDto);
    }

    public async Task<PacoteDto> CriarAsync(CriarPacoteDto dto, CancellationToken cancellationToken = default)
    {
        var servicos = new List<Servico>();
        foreach (var sid in dto.ServicoIds)
        {
            var s = await _servicoRepo.ObterPorIdAsync(sid, cancellationToken)
                ?? throw new KeyNotFoundException($"Servico {sid} nao encontrado.");
            servicos.Add(s);
        }

        var pacote = new Pacote
        {
            Nome = dto.Nome,
            PrecoTotal = dto.PrecoTotal,
            Ativo = true,
            Itens = servicos.Select(s => new PacoteItem { ServicoId = s.Id }).ToList()
        };

        await _pacoteRepo.AdicionarAsync(pacote, cancellationToken);

        var salvo = await _pacoteRepo.ObterPorIdComItensAsync(pacote.Id, cancellationToken);
        return MapToDto(salvo!);
    }

    public async Task<PacoteDto> AtualizarAsync(int id, AtualizarPacoteDto dto, CancellationToken cancellationToken = default)
    {
        var pacote = await _pacoteRepo.ObterPorIdComItensAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pacote {id} nao encontrado.");

        var servicos = new List<Servico>();
        foreach (var sid in dto.ServicoIds)
        {
            var s = await _servicoRepo.ObterPorIdAsync(sid, cancellationToken)
                ?? throw new KeyNotFoundException($"Servico {sid} nao encontrado.");
            servicos.Add(s);
        }

        pacote.Nome = dto.Nome;
        pacote.PrecoTotal = dto.PrecoTotal;
        pacote.Ativo = dto.Ativo;
        pacote.Itens = servicos.Select(s => new PacoteItem { ServicoId = s.Id, PacoteId = pacote.Id }).ToList();

        await _pacoteRepo.AtualizarAsync(pacote, cancellationToken);

        var atualizado = await _pacoteRepo.ObterPorIdComItensAsync(pacote.Id, cancellationToken);
        return MapToDto(atualizado!);
    }

    public async Task DesativarAsync(int id, CancellationToken cancellationToken = default)
    {
        var pacote = await _pacoteRepo.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pacote {id} nao encontrado.");
        pacote.Ativo = false;
        await _pacoteRepo.AtualizarAsync(pacote, cancellationToken);
    }

    public async Task AtivarAsync(int id, CancellationToken cancellationToken = default)
    {
        var pacote = await _pacoteRepo.ObterPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Pacote {id} nao encontrado.");
        pacote.Ativo = true;
        await _pacoteRepo.AtualizarAsync(pacote, cancellationToken);
    }

    private static PacoteDto MapToDto(Pacote p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        PrecoTotal = p.PrecoTotal,
        Ativo = p.Ativo,
        Servicos = p.Itens
            .Where(i => i.Servico != null)
            .Select(i => new ServicoDto
            {
                Id = i.Servico!.Id,
                Nome = i.Servico.Nome,
                Descricao = i.Servico.Descricao,
                Preco = i.Servico.Preco,
                DuracaoMinutos = i.Servico.DuracaoMinutos,
                Ativo = i.Servico.Ativo,
                ExibirNoSite = i.Servico.ExibirNoSite
            }).ToList()
    };
}
