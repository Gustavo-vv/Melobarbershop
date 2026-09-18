// ============================================================================
// Arquivo: UsuarioService.cs
// Camada: Melobarbershop.Application (Serviços - Implementações)
// Objetivo: Implementar as regras de negócio de usuários (clientes, barbeiros, recepcionistas, admins),
//           integração com ASP.NET Core Identity e controle de bloqueios de agenda de profissionais.
// Papel na Arquitetura:
//   - Faz ponte com IUsuarioRepository e UserManager<ApplicationUser>.
//   - Valida duplicidade de e-mail e telefone de WhatsApp.
//   - Gerencia bloqueios de expediente (folgas, pausas, férias) de barbeiros.
// ============================================================================

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Application.Servicos.Services;

namespace Melobarbershop.Application.Servicos.Implementacoes;

/// <summary>
/// Implementação do serviço de gestão de usuários e bloqueios de agenda.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor com injeção do repositório de usuários, UserManager do Identity e AutoMapper.
    /// </summary>
    public UsuarioService(
        IUsuarioRepository usuarioRepo,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _usuarioRepo = usuarioRepo;
        _userManager = userManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém um usuário pelo ID único, carregando também os seus perfis/roles do Identity.
    /// </summary>
    public async Task<UsuarioDto?> ObterPorIdAsync(string id)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id);
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

    /// <summary>
    /// Localiza um usuário pelo número de telefone cadastrado.
    /// </summary>
    public async Task<UsuarioDto?> ObterPorTelefoneAsync(string telefone)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorTelefoneAsync(telefone);
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

    /// <summary>
    /// Localiza um usuário pelo e-mail cadastrado.
    /// </summary>
    public async Task<UsuarioDto?> ObterPorEmailAsync(string email)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorEmailAsync(email);
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

    /// <summary>
    /// Lista os usuários que pertencem a um perfil/role específico (ex: Barbeiro, Cliente, Admin).
    /// </summary>
    public async Task<IEnumerable<UsuarioDto>> ListarPorRoleAsync(string roleName, bool apenasAtivos = true)
    {
        try
        {
            IEnumerable<ApplicationUser> usuarios = apenasAtivos
                ? await _usuarioRepo.ObterAtivosPorRoleAsync(roleName)
                : await _usuarioRepo.ObterPorRoleAsync(roleName);

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

    /// <summary>
    /// Cria um novo usuário cadastrando suas credenciais no ASP.NET Core Identity com a role padrão Cliente.
    /// </summary>
    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto)
    {
        try
        {
            // Valida unicidade de e-mail e telefone
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
                DataCadastro = DateTime.UtcNow,
                Ativo = true
            };

            var result = await _userManager.CreateAsync(usuario, dto.Senha);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Erro ao criar usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            const string roleCliente = "Cliente"; // sempre Cliente para autoregistro
            await _userManager.AddToRoleAsync(usuario, roleCliente);

            var resultDto = _mapper.Map<UsuarioDto>(usuario);
            resultDto.Roles = [roleCliente];
            return resultDto;
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao criar usuario.", ex);
        }
    }

    /// <summary>
    /// Atualiza os dados cadastrais de um usuário, validando conflito de telefone com outros registros.
    /// </summary>
    public async Task<UsuarioDto> AtualizarAsync(string id, AtualizarUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id);
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

            await _usuarioRepo.AtualizarAsync(usuario);

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

    /// <summary>
    /// Desativa o usuário (exclusão lógica), impedindo login e novos agendamentos.
    /// </summary>
    public async Task DesativarAsync(string id)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario '{id}' nao encontrado.");

            usuario.Ativo = false;
            await _usuarioRepo.AtualizarAsync(usuario);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao desativar usuario '{id}'.", ex);
        }
    }

    /// <summary>
    /// Reativa o cadastro de um usuário previamente desativado.
    /// </summary>
    public async Task AtivarAsync(string id)
    {
        try
        {
            var usuario = await _usuarioRepo.ObterPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario '{id}' nao encontrado.");

            usuario.Ativo = true;
            await _usuarioRepo.AtualizarAsync(usuario);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao ativar usuario '{id}'.", ex);
        }
    }

    /// <summary>
    /// Cadastra um bloqueio na agenda de um barbeiro (ex.: intervalo de almoço, folga ou médico).
    /// </summary>
    public async Task<BloqueioAgendaDto> AdicionarBloqueioAgendaAsync(CriarBloqueioAgendaDto dto)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(dto.BarbeiroId);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{dto.BarbeiroId}' nao encontrado.");

            var bloqueio = new BloqueioAgenda
            {
                BarbeiroId = dto.BarbeiroId,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dto.DataHoraFim,
                Motivo = dto.Motivo
            };

            await _usuarioRepo.AdicionarBloqueioAsync(bloqueio);

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

    /// <summary>
    /// Remove um bloqueio de agenda liberando os horários para agendamento.
    /// </summary>
    public async Task RemoverBloqueioAgendaAsync(int bloqueioId)
    {
        try
        {
            var bloqueio = await _usuarioRepo.ObterBloqueioPorIdAsync(bloqueioId);
            if (bloqueio == null)
                throw new KeyNotFoundException($"Bloqueio '{bloqueioId}' nao encontrado.");

            await _usuarioRepo.RemoverBloqueioAsync(bloqueio);
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao remover bloqueio '{bloqueioId}'.", ex);
        }
    }

    /// <summary>
    /// Lista os bloqueios de agenda registrados para um barbeiro em um período específico.
    /// </summary>
    public async Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var bloqueios = await _usuarioRepo.ObterBloqueiosPorPeriodoAsync(barbeiroId, inicio, fim);
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

    /// <summary>
    /// Checa se o barbeiro está livre (sem bloqueios de agenda) em um determinado intervalo.
    /// </summary>
    public async Task<bool> VerificarDisponibilidadeBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        try
        {
            var barbeiro = await _usuarioRepo.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro '{barbeiroId}' nao encontrado.");

            var temBloqueio = await _usuarioRepo.ExisteBloqueioNoPeriodoAsync(barbeiroId, inicio, fim);
            return !temBloqueio;
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao verificar disponibilidade do barbeiro '{barbeiroId}'.", ex);
        }
    }
}
