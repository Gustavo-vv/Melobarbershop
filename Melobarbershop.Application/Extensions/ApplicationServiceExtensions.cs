using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Melobarbershop.Application.Interfaces.Services;
using Melobarbershop.Application.Mapeamentos;
using Melobarbershop.Application.Servicos.Implementacoes;

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

        // Serviços de Aplicação
        services.AddScoped<IServicoService, ServicoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();

        return services;
    }
}
