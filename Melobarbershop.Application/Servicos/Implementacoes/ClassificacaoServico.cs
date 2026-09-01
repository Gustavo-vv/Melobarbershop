// Nome do arquivo: ClassificacaoServico.cs
// Objetivo: Implementacao do servico de classificacao indicativa
// Camada: Application
// Como participa: Retorna as classificacoes convertidas para DTO

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;
using SenacFlix.Domain.Interfaces;

namespace SenacFlix.Application.Servicos.Implementacoes
{
    public class ClassificacaoServico : IClassificacaoServico
    {
        private readonly IClassificacaoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public ClassificacaoServico(IClassificacaoRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ApiResposta<IEnumerable<ClassificacaoDto>>> ObterTodasAsync()
        {
            try
            {
                var classificacoes = await _repositorio.ObterTodasAsync();
                var dtos = _mapper.Map<IEnumerable<ClassificacaoDto>>(classificacoes);
                return ApiResposta<IEnumerable<ClassificacaoDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<ClassificacaoDto>>.Falha($"Erro ao obter classificacoes: {ex.Message}");
            }
        }
    }
}
