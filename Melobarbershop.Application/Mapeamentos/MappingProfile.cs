// ============================================================================
// Arquivo: MappingProfile.cs
// Camada: Melobarbershop.Application (Mapeamentos)
// Objetivo: Configurar as regras declarativas de conversão de dados entre as entidades
//           de domínio (Domain) e os objetos de transferência de dados (DTOs) via AutoMapper.
// Papel na Arquitetura:
//   - Centraliza o mapeamento de campos com mesmo nome por convenção automática.
//   - Customiza mapeamentos complexos (flattening de relacionamentos, somatórios de valores,
//     projeção de coleções aninhadas como em Pacote e Agendamento).
//   - Protege propriedades sensíveis ignorando atributos que não devem ser alterados
//     diretamente em comandos de atualização (como chaves primárias ou saldos de estoque).
// ============================================================================

using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Application.Mapeamentos;

/// <summary>
/// Perfil de configuração do AutoMapper para todo o domínio da Melobarbershop.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Construtor onde são registrados todos os mapeamentos bidirecionais ou unidirecionais.
    /// </summary>
    public MappingProfile()
    {
        // --------------------------------------------------------------------
        // Mapeamentos de Serviço
        // --------------------------------------------------------------------
        // Converte entidade Servico para ServicoDto (projeção direta de propriedades compatíveis)
        CreateMap<Servico, ServicoDto>();

        // Converte CriarServicoDto para Servico:
        // - Ignora Id (gerado automaticamente pelo banco / Identity Specification)
        // - Define Ativo como true por padrão na inclusão
        CreateMap<CriarServicoDto, Servico>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(_ => true));

        // Converte AtualizarServicoDto para Servico:
        // - Ignora Id para garantir que o identificador da rota/URL permaneça soberano
        CreateMap<AtualizarServicoDto, Servico>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // --------------------------------------------------------------------
        // Mapeamentos de Pacote / Combo
        // --------------------------------------------------------------------
        // Extrai a lista de entidades Servico aninhadas dentro da coleção PacoteItem
        // transformando-a em uma coleção limpa de ServicoDto
        CreateMap<Pacote, PacoteDto>()
            .ForMember(dest => dest.Servicos, opt => opt.MapFrom(
                src => src.Itens != null
                    ? src.Itens.Where(i => i.Servico != null).Select(i => i.Servico)
                    : Enumerable.Empty<Servico>()));

        // --------------------------------------------------------------------
        // Mapeamentos de Produto e Estoque
        // --------------------------------------------------------------------
        // Mapeia Produto para ProdutoDto calculando o flag de alerta EstoqueBaixo
        CreateMap<Produto, ProdutoDto>()
            .ForMember(dest => dest.EstoqueBaixo, opt => opt.MapFrom(src => src.EstoqueAtual <= src.EstoqueMinimoAlerta));

        // Converte CriarProdutoDto para Produto:
        // - Inicializa EstoqueAtual com o valor informado em EstoqueInicial
        // - Marca Ativo = true
        // - Ignora Movimentacoes (serão inseridas via histórico separado)
        CreateMap<CriarProdutoDto, Produto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EstoqueAtual, opt => opt.MapFrom(src => src.EstoqueInicial))
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());

        // Converte AtualizarProdutoDto para Produto:
        // - Impede a edição direta do campo EstoqueAtual via atualização de cadastro,
        //   forçando qualquer alteração de saldo a passar por movimentações auditadas.
        CreateMap<AtualizarProdutoDto, Produto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EstoqueAtual, opt => opt.Ignore())
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());

        // --------------------------------------------------------------------
        // Mapeamentos de Movimentação de Estoque
        // --------------------------------------------------------------------
        // Achata (flatten) a navegação Produto.Nome para a propriedade direta NomeProduto
        CreateMap<MovimentacaoEstoque, MovimentacaoEstoqueDto>()
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty));

        // --------------------------------------------------------------------
        // Mapeamentos de Agendamento
        // --------------------------------------------------------------------
        // Projeta propriedades de navegação para campos simples (Cliente.Nome, Cliente.PhoneNumber, Barbeiro.Nome)
        // e calcula em tempo de execução o ValorTotal a partir da soma dos itens cadastrados.
        CreateMap<Agendamento, AgendamentoDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.TelefoneCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.PhoneNumber : null))
            .ForMember(dest => dest.NomeBarbeiro, opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : string.Empty))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.Itens != null ? src.Itens.Sum(i => i.PrecoCobrado) : 0m));

        // Mapeia AgendamentoItem achatando o Servico.Nome
        CreateMap<AgendamentoItem, AgendamentoItemDto>()
            .ForMember(dest => dest.NomeServico, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : string.Empty));

        // --------------------------------------------------------------------
        // Mapeamentos de Venda / PDV
        // --------------------------------------------------------------------
        // Projeta o NomeCliente e totaliza os pagamentos parciais para compor ValorPago
        CreateMap<Venda, VendaDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : null))
            .ForMember(dest => dest.ValorPago, opt => opt.MapFrom(src => src.Pagamentos != null ? src.Pagamentos.Sum(p => p.Valor) : 0m));

        // Mapeia VendaItem achatando os nomes do Serviço, Produto ou Barbeiro associados
        CreateMap<VendaItem, VendaItemDto>()
            .ForMember(dest => dest.NomeServico, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : null))
            .ForMember(dest => dest.NomeBarbeiro, opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : null));

        // Mapeamento direto de Pagamento para PagamentoDto
        CreateMap<Pagamento, PagamentoDto>();

        // --------------------------------------------------------------------
        // Mapeamento de Usuário Identity
        // --------------------------------------------------------------------
        // Mapeia ApplicationUser para UsuarioDto:
        // As Roles do Identity requerem chamada assíncrona ao UserManager.GetRolesAsync,
        // por isso são ignoradas aqui e preenchidas no UsuarioService.
        CreateMap<ApplicationUser, UsuarioDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        // --------------------------------------------------------------------
        // Mapeamento de Avaliação
        // --------------------------------------------------------------------
        // Projeta o nome do cliente e do barbeiro a partir das propriedades de navegação
        CreateMap<Avaliacao, AvaliacaoDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.NomeBarbeiro, opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : string.Empty));
    }
}