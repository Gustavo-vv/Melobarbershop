// Nome do arquivo: IClassificacaoServico.cs
// Objetivo: Interface para o servico de classificacoes indicativas
// Camada: Application
// Como participa: Servico de leitura para obter as classificacoes via DTO

using System.Collections.Generic;
using System.Threading.Tasks;
using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface IClassificacaoServico
    {
        Task<ApiResposta<IEnumerable<ClassificacaoDto>>> ObterTodasAsync();
    }
}
