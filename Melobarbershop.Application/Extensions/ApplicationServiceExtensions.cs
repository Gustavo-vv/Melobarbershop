using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Melobarbershop.Application.Mapeamentos;
using Melobarbershop.Application.Servicos.Implementacoes;
using Melobarbershop.Application.Servicos.Services;

namespace Melobarbershop.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper Configuration
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, NullLoggerFactory.Instance);
        services.AddSingleton<IMapper>(mapperConfig.CreateMapper());

        // ServiÃ§os de AplicaÃ§Ã£o
        services.AddScoped<IServicoService, ServicoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();
        services.AddScoped<IVendaService, VendaService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IPacoteService, PacoteService>();
        services.AddScoped<IAvaliacaoService, AvaliacaoService>();

        return services;
    }
}
