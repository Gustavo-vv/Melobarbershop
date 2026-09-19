// ============================================================================
// Arquivo: DbSeeder.cs
// Camada: Melobarbershop.Infrastructure (Data)
// Objetivo: Inicialização de sementes essenciais de segurança e acesso do sistema (Roles e Superusuário).
// Papel na Arquitetura:
//   - Cria roles padronizadas da aplicação (Admin, Barbeiro, Cliente) usando ASP.NET Identity.
//   - Cria a conta padrão do Administrador do sistema caso ela ainda não exista.
// ============================================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Infrastructure.Data;

/// <summary>
/// Responsável por criar as roles iniciais e o usuário Admin padrão.
/// Chamado durante o pipeline de inicialização da aplicação (Program.cs).
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Constantes com os nomes das Roles (perfis de acesso) suportados pelo sistema.
    /// </summary>
    public static class Roles
    {
        /// <summary>Perfil de Administrador do sistema com acesso irrestrito.</summary>
        public const string Admin = "Admin";

        /// <summary>Perfil de Barbeiro/Profissional com acesso à agenda e comandas de atendimento.</summary>
        public const string Barbeiro = "Barbeiro";

        /// <summary>Perfil de Cliente com permissões de autoagendamento e histórico de serviços.</summary>
        public const string Cliente = "Cliente";

        /// <summary>Array auxiliar contendo todos os perfis disponíveis no sistema.</summary>
        public static readonly string[] Todos = [Admin, Barbeiro, Cliente];
    }

    /// <summary>
    /// Executa a rotina assíncrona de criação de roles e do usuário administrador padrão caso não existam.
    /// </summary>
    /// <param name="serviceProvider">Provedor de serviços para resolução de RoleManager e UserManager.</param>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = serviceProvider.GetRequiredService<ILogger<BarbeariaDbContext>>();

        // 1. Criar as Roles se não existirem
        foreach (var role in Roles.Todos)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                    logger.LogInformation($"Role '{role}' criada com sucesso.");
                else
                    logger.LogError($"Erro ao criar role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // 2. Criar o usuário Admin padrão se não existir
        const string adminEmail = "admin@melobarbershop.com";
        const string adminSenha = "Admin@123";

        var adminExistente = await userManager.FindByEmailAsync(adminEmail);
        if (adminExistente == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nome = "Administrador",
                PhoneNumber = "(11) 98888-0001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, adminSenha);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                logger.LogInformation($"Usuário Admin criado: {adminEmail}");
            }
            else
            {
                logger.LogError($"Erro ao criar Admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
