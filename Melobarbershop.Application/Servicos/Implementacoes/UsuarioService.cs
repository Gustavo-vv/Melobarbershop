using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Application.Servicos.Services;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UsuarioService(
        IUsuarioRepository usuarioRepo,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _usuarioRepo = usuarioRepo;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id, cancellationToken);
            if (usuario == null) return null;
            var dto = _mapper.Map<UsuarioDto>(usuario);
            dto.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            return dto;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter usuario com ID '{id}'.", ex);
        }
    }

    public async Task<UsuarioDto?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorTelefoneAsync(telefone, cancellationToken);
            if (usuario == null) return null;
            var dto = _mapper.Map<UsuarioDto>(usuario);
            dto.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            return dto;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter usuario com telefone '{telefone}'.", ex);
        }
    }

    public async Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorEmailAsync(email, cancellationToken);
            if (usuario == null) return null;
            var dto = _mapper.Map<UsuarioDto>(usuario);
            dto.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            return dto;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter usuario com e-mail '{email}'.", ex);
        }
    }

    public async Task<IEnumerable<UsuarioDto>> ListarPorRoleAsync(string roleName, bool apenasAtivos = true, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<ApplicationUser> usuarios = apenasAtivos
                ? await _usuarioRepo.ObterAtivosPorRoleAsync(roleName, cancellationToken)
                : await _usuarioRepo.ObterPorRoleAsync(roleName, cancellationToken);

            var dtos = new List<UsuarioDto>();
            foreach (var u in usuarios)
            {
                var dto = _mapper.Map<UsuarioDto>(u);
                dto.Roles = (await _userManager.GetRolesAsync(u)).ToList();
                dtos.Add(dto);
            }
            return dtos;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar usuarios com perfil '{roleName}'.", ex);
        }
    }

    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _usuarioRepo.ExisteEmailAsync(dto.Email))
                throw new InvalidOperationException($"Ja existe um usuario com o e-mail '{dto.Email}'.");

            if (!string.IsNullOrWhiteSpace(dto.TelefoneWhatsApp) && await _usuarioRepo.ExisteTelefoneAsync(dto.TelefoneWhatsApp))
                throw new InvalidOperationException($"Ja existe um usuario com o telefone '{dto.TelefoneWhatsApp}'.");

            var usuario = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nome = dto.Nome,
                PhoneNumber = dto.TelefoneWhatsApp,
                DataNascimento = dto.DataNascimento,
                PreferenciasNotas = dto.PreferenciasNotas,
                FotoUrl = dto.FotoUrl,
                PercentualComissao = dto.PercentualComissao,
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };

            var result = await _userManager.CreateAsync(usuario, dto.Senha);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Erro ao criar usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var roleValida = dto.Role is "Cliente" or "Barbeiro" or "Admin" ? dto.Role : "Cliente";
            await _userManager.AddToRoleAsync(usuario, roleValida);

            var resultDto = _mapper.Map<UsuarioDto>(usuario);
            resultDto.Roles = [roleValida];
            return resultDto;
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao criar usuario.", ex);
        }
    }

    public async Task<UsuarioDto> AtualizarAsync(string id, AtualizarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id, cancellationToken);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario '{id}' nao encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.TelefoneWhatsApp) && await _usuarioRepo.ExisteTelefoneAsync(dto.TelefoneWhatsApp, id))
                throw new InvalidOperationException($"Ja existe outro usuario com o telefone '{dto.TelefoneWhatsApp}'.");

            usuario.Nome = dto.Nome;
            usuario.PhoneNumber = dto.TelefoneWhatsApp;
            usuario.DataNascimento = dto.DataNascimento;
            usuario.PreferenciasNotas = dto.PreferenciasNotas;
            usuario.FotoUrl = dto.FotoUrl;
            usuario.PercentualComissao = dto.PercentualComissao;
            usuario.Ativo = dto.Ativo;

            await _usuarioRepo.AtualizarAsync(usuario, cancellationToken);

            var resultDto = _mapper.Map<UsuarioDto>(usuario);
            resultDto.Roles = (await _userManager.GetRolesAsync(usuario)).ToList();
            return resultDto;
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao atualizar usuario '{id}'.", ex);
        }
    }

    public async Task DesativarAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id, cancellationToken);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario '{id}' nao encontrado.");

            usuario.Ativo = false;
            await _usuarioRepo.AtualizarAsync(usuario, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao desativar usuario '{id}'.", ex);
        }
    }

    public async Task AtivarAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id, cancellationToken);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario '{id}' nao encontrado.");

            usuario.Ativo = true;
            await _usuarioRepo.AtualizarAsync(usuario, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao ativar usuario '{id}'.", ex);
        }
    }

    public async Task<BloqueioAgendaDto> AdicionarBloqueioAgendaAsync(CriarBloqueioAgendaDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(dto.BarbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{dto.BarbeiroId}' nao encontrado.");

            var bloqueio = new BloqueioAgenda
            {
                BarbeiroId = dto.BarbeiroId,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dto.DataHoraFim,
                Motivo = dto.Motivo
            };

            await _usuarioRepo.AdicionarBloqueioAsync(bloqueio, cancellationToken);

            return new BloqueioAgendaDto
            {
                Id = bloqueio.Id,
                BarbeiroId = bloqueio.BarbeiroId,
                NomeBarbeiro = barbeiro.Nome,
                DataHoraInicio = bloqueio.DataHoraInicio,
                DataHoraFim = bloqueio.DataHoraFim,
                Motivo = bloqueio.Motivo
            };
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao adicionar bloqueio de agenda.", ex);
        }
    }

    public async Task RemoverBloqueioAgendaAsync(int bloqueioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var bloqueio = await _usuarioRepo.ObterBloqueioPorIdAsync(bloqueioId, cancellationToken);
            if (bloqueio == null)
                throw new KeyNotFoundException($"Bloqueio '{bloqueioId}' nao encontrado.");

            await _usuarioRepo.RemoverBloqueioAsync(bloqueio, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao remover bloqueio '{bloqueioId}'.", ex);
        }
    }

    public async Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var bloqueios = await _usuarioRepo.ObterBloqueiosPorPeriodoAsync(barbeiroId, inicio, fim, cancellationToken);
            return bloqueios.Select(b => new BloqueioAgendaDto
            {
                Id = b.Id,
                BarbeiroId = b.BarbeiroId,
                NomeBarbeiro = barbeiro.Nome,
                DataHoraInicio = b.DataHoraInicio,
                DataHoraFim = b.DataHoraFim,
                Motivo = b.Motivo
            });
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar bloqueios do barbeiro '{barbeiroId}'.", ex);
        }
    }

    public async Task<bool> VerificarDisponibilidadeBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var temBloqueio = await _usuarioRepo.ExisteBloqueioNoPeriodoAsync(barbeiroId, inicio, fim, cancellationToken);
            return !temBloqueio;
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao verificar disponibilidade do barbeiro '{barbeiroId}'.", ex);
        }
    }
}
