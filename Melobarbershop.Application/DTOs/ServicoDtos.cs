// ============================================================================
// Arquivo: ServicoDtos.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Definir os contratos de transferência para o catálogo de serviços
//           oferecidos pela barbearia (cortes, barba, pigmentação, etc.).
// Papel na Arquitetura:
//   - Isola a entidade de domínio Servico de parâmetros de entrada e saída.
//   - Carrega tempo de duração estimado para alimentar os algoritmos de cálculo
//     de horários disponíveis na agenda dos barbeiros.
// ============================================================================

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// DTO de leitura contendo os dados completos de um serviço para exibição na agenda, catálogo e site.
/// </summary>
public class ServicoDto
{
    /// <summary>Identificador único do serviço.</summary>
    public int Id { get; set; }

    /// <summary>Nome comercial do serviço (ex: "Corte Degradê", "Barba Terapia").</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Descrição detalhada dos procedimentos ou benefícios do serviço.</summary>
    public string? Descricao { get; set; }

    /// <summary>Preço padrão cobrado pelo serviço.</summary>
    public decimal Preco { get; set; }

    /// <summary>Duração média em minutos (utilizada para reservar o bloco de tempo na agenda).</summary>
    public int DuracaoMinutos { get; set; }

    /// <summary>Indica se o serviço está ativo para agendamento na barbearia.</summary>
    public bool Ativo { get; set; }

    /// <summary>Indica se o serviço deve ser listado na página pública de agendamentos do cliente.</summary>
    public bool ExibirNoSite { get; set; }
}

/// <summary>
/// DTO de entrada para cadastrar um novo serviço no catálogo do sistema.
/// </summary>
public class CriarServicoDto
{
    /// <summary>Nome do novo serviço.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Descrição opcional do procedimento.</summary>
    public string? Descricao { get; set; }

    /// <summary>Preço padrão em Reais.</summary>
    public decimal Preco { get; set; }

    /// <summary>Tempo estimado em minutos necessário para a execução.</summary>
    public int DuracaoMinutos { get; set; }

    /// <summary>Disponibilidade imediata na vitrine web (padrão true).</summary>
    public bool ExibirNoSite { get; set; } = true;
}

/// <summary>
/// DTO de entrada para alteração cadastral ou ajuste de preço/tempo de um serviço existente.
/// </summary>
public class AtualizarServicoDto
{
    /// <summary>Nome atualizado do serviço.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Descrição atualizada do serviço.</summary>
    public string? Descricao { get; set; }

    /// <summary>Novo preço padrão a ser praticado.</summary>
    public decimal Preco { get; set; }

    /// <summary>Novo tempo estimado de execução em minutos.</summary>
    public int DuracaoMinutos { get; set; }

    /// <summary>Indica se o serviço permanece ativo para agendamento.</summary>
    public bool Ativo { get; set; } = true;

    /// <summary>Indica se o serviço continua visível na página pública da web.</summary>
    public bool ExibirNoSite { get; set; } = true;
}

