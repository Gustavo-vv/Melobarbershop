// ============================================================================
// Arquivo: PacoteDtos.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Definir os contratos de dados para gerenciamento de combos/pacotes de serviços.
// Papel na Arquitetura:
//   - Isola as entidades de domínio Pacote e PacoteItem das requisições e respostas de API.
//   - Permite que o cliente cadastre um pacote enviando apenas uma lista de IDs de serviços,
//     enquanto a visualização devolve os detalhes completos dos serviços vinculados.
// ============================================================================

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// DTO de saída com todos os dados de um pacote comercial, incluindo a coleção de serviços incluídos.
/// </summary>
public class PacoteDto
{
    /// <summary>Identificador primário único do pacote.</summary>
    public int Id { get; set; }

    /// <summary>Nome comercial do pacote (ex: "Combo Barba & Cabelo").</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Preço promocional ou fechado cobrado pela execução do pacote completo.</summary>
    public decimal PrecoTotal { get; set; }

    /// <summary>Indica se o pacote está disponível para venda e agendamento.</summary>
    public bool Ativo { get; set; }

    /// <summary>Coleção dos serviços individuais que compõem este combo promocional.</summary>
    public ICollection<ServicoDto> Servicos { get; set; } = new List<ServicoDto>();
}

/// <summary>
/// DTO de entrada para cadastrar um novo pacote promocional na barbearia.
/// </summary>
public class CriarPacoteDto
{
    /// <summary>Nome do novo combo a ser criado.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Preço de venda definido para o pacote promocional.</summary>
    public decimal PrecoTotal { get; set; }

    /// <summary>Lista com os IDs dos serviços existentes que farão parte do pacote.</summary>
    public ICollection<int> ServicoIds { get; set; } = new List<int>();
}

/// <summary>
/// DTO de entrada para atualizar os dados ou os itens de um pacote existente.
/// </summary>
public class AtualizarPacoteDto
{
    /// <summary>Novo nome do pacote.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Novo preço do pacote promocional.</summary>
    public decimal PrecoTotal { get; set; }

    /// <summary>Lista atualizada com os IDs dos serviços que devem compor o pacote.</summary>
    public ICollection<int> ServicoIds { get; set; } = new List<int>();

    /// <summary>Define se o pacote permanece ativo ou se é descontinuado.</summary>
    public bool Ativo { get; set; } = true;
}

