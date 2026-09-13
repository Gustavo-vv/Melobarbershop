using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Infrastructure.Data;

/// <summary>
/// Responsável por criar as roles iniciais e o usuário Admin padrão.
/// Chamado uma única vez na inicialização da aplicação.
/// </summary>
public static class DbSeeder
{
    // Nomes das roles do sistema
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Barbeiro = "Barbeiro";
        public const string Cliente = "Cliente";

        public static readonly string[] Todos = [Admin, Barbeiro, Cliente];
    }

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
