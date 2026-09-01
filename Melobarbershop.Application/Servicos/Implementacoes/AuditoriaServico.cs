// Nome do arquivo: AuditoriaServico.cs
// Objetivo: Implementacao do servico de auditoria
// Camada: Application
// Como participa: Registra e retorna logs do sistema

using AutoMapper;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;

namespace SenacFlix.Application.Servicos.Implementacoes
{
    public class AuditoriaServico : IAuditoriaServico
    {
        private readonly IAuditoriaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public AuditoriaServico(IAuditoriaRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ApiResposta<IEnumerable<AuditoriaDto>>> ObterTodasAsync()
        {
            try
            {
                var registros = await _repositorio.ObterTodasAsync();
                var dtos = _mapper.Map<IEnumerable<AuditoriaDto>>(registros);
                return ApiResposta<IEnumerable<AuditoriaDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<AuditoriaDto>>.Falha($"Erro {ex.Message}");
            }
        }

        public async Task RegistrarAsync(string usuarioId, string nomeUsuario, string acao, string tabela, string? detalhes = null)
        {
            var auditoria = new Auditoria
            {
                UsuarioId = usuarioId,

                NomeUsuario = nomeUsuario,

                Acao = acao,

                TabelaAfetada = tabela,

                Detalhes = detalhes,

                DataHora = DateTime.UtcNow

            };


            await _repositorio.RegistrarAsync(auditoria);

        }
    }
}
