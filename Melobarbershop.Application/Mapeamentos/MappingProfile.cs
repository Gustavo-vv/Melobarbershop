using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Application.Mapeamentos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Serviço
        CreateMap<Servico, ServicoDto>();
        CreateMap<CriarServicoDto, Servico>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(_ => true));
        CreateMap<AtualizarServicoDto, Servico>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Produto
        CreateMap<Produto, ProdutoDto>()
            .ForMember(dest => dest.EstoqueBaixo, opt => opt.MapFrom(src => src.EstoqueAtual <= src.EstoqueMinimoAlerta));
        CreateMap<CriarProdutoDto, Produto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EstoqueAtual, opt => opt.MapFrom(src => src.EstoqueInicial))
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());
        CreateMap<AtualizarProdutoDto, Produto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EstoqueAtual, opt => opt.Ignore())
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());

        // Movimentação de Estoque
        CreateMap<MovimentacaoEstoque, MovimentacaoEstoqueDto>()
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty));

        // Agendamento
        CreateMap<Agendamento, AgendamentoDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.TelefoneCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.PhoneNumber : null))
            .ForMember(dest => dest.NomeBarbeiro, opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : string.Empty))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.Itens != null ? src.Itens.Sum(i => i.PrecoCobrado) : 0m));

        // Agendamento Item
        CreateMap<AgendamentoItem, AgendamentoItemDto>()
            .ForMember(dest => dest.NomeServico, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : string.Empty));

        // Venda
        CreateMap<Venda, VendaDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : null))
            .ForMember(dest => dest.ValorPago, opt => opt.MapFrom(src => src.Pagamentos != null ? src.Pagamentos.Sum(p => p.Valor) : 0m));

        // Venda Item
        CreateMap<VendaItem, VendaItemDto>()
            .ForMember(dest => dest.NomeServico, opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : null))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : null))
            .ForMember(dest => dest.NomeBarbeiro, opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : null));

        // Pagamento
        CreateMap<Pagamento, PagamentoDto>();
    }
}
