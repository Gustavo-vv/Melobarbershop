using Melobarbershop.Infrastructure.Data;
using Melobarbershop.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// SERVICES
// ==========================================

// Infrastructure: DbContext + Identity (com Roles) + Repositories
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// SEED: Roles e Admin padrão
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Aplica migrations pendentes automaticamente em desenvolvimento
    var db = services.GetRequiredService<BarbeariaDbContext>();
    await db.Database.MigrateAsync();

    // Cria roles e usuário admin inicial
    await DbSeeder.SeedAsync(services);
}

// ==========================================
// PIPELINE
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
