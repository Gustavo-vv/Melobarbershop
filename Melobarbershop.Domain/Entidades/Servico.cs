// Arquivo: Melobarbershop.Domain/Entidades/Servico.cs
// Namespace: Melobarbershop.Domain.Entidades
// Conteúdo: class Servico
// Resumo: Representa um serviço oferecido (nome, preço, duração e visibilidade no site).
namespace Melobarbershop.Domain.Entidades;
public class Servico
{
   
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; } // Ex: 30, 45, 60
    public bool Ativo { get; set; } = true;
    public bool ExibirNoSite { get; set; } = true;
}

