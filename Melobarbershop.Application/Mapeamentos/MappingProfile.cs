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
    }
}
