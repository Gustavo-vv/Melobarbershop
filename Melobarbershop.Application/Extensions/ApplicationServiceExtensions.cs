// ============================================================================
// Arquivo: ApplicationServiceExtensions.cs
// Camada: Melobarbershop.Application (Extensions)
// Objetivo: Centralizar o registro de serviços e configurações da camada de Aplicação
//           no contêiner de Injeção de Dependência nativo do .NET (IServiceCollection).
// Papel na Arquitetura:
//   - Padrão Extension Method para IServiceCollection, permitindo que a API e demais hosts
//     adicionem toda a camada de aplicação com uma única chamada: `builder.Services.AddApplication()`.
//   - Instancia e valida a integridade do AutoMapper (AssertConfigurationIsValid) na inicialização.
//   - Registra os serviços de negócio como Scoped (um ciclo de vida por requisição HTTP).
// ============================================================================

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Melobarbershop.Application.Mapeamentos;
using Melobarbershop.Application.Servicos.Implementacoes;
using Melobarbershop.Application.Servicos.Services;

namespace Melobarbershop.Application.Extensions;

/// <summary>
/// Métodos de extensão para configuração dos serviços da camada de aplicação no pipeline de DI.
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registra o AutoMapper com perfil validado e todos os serviços de aplicação com ciclo de vida Scoped.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <returns>A própria coleção de serviços para encadeamento fluente.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // --------------------------------------------------------------------
        // Configuração e Validação do AutoMapper
        // --------------------------------------------------------------------
        // MapperConfiguration carrega o perfil MappingProfile.
        // O uso do NullLoggerFactory evita alocação desnecessária de loggers durante o setup.
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, NullLoggerFactory.Instance);

        // Fail-Fast: Valida se todos os membros de destino mapeados possuem origem correspondente,
        // disparando erro na subida da aplicação caso falte configuração em algum DTO.
        mapperConfig.AssertConfigurationIsValid();

        // O IMapper é thread-safe e imutável após construído; logo, registra-se como Singleton.
        services.AddSingleton<IMapper>(mapperConfig.CreateMapper());

        // --------------------------------------------------------------------
        // Registro dos Serviços de Aplicação (Scoped)
        // --------------------------------------------------------------------
        // Registrados como Scoped para compartilhar a mesma instância de DbContext
        // e transação por requisição HTTP (ou operação de UI).
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

