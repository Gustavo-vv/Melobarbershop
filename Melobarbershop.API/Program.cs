using Melobarbershop.Application.Extensions;
using Melobarbershop.Infrastructure.Data;
using Melobarbershop.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// SERVICES
// ==========================================

// Infrastructure: DbContext + Identity (com Roles) + Repositories
builder.Services.AddInfrastructure(builder.Configuration);

// Application: AutoMapper + Serviços de Domínio/Aplicação
builder.Services.AddApplication();

// Controllers
builder.Services.AddControllers();

// --- Autenticação JWT (ANTES estava faltando) ---
var chaveJwt = builder.Configuration["Jwt:Chave"] ?? "MelobarbershopChaveSecretaSuperSegura2024!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Emissor"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audiencia"],
        ClockSkew = TimeSpan.Zero,
    };
});

// --- Autorização (OBRIGATÓRIO — sem isso, UseAuthorization() derruba a API) ---
builder.Services.AddAuthorization();

// --- CORS, para permitir chamadas de front-end/desktop/mobile ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("MelobarbershopCors", policy =>
    {
        policy.WithOrigins("http://localhost:5002", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Melobarbershop API",
        Version = "v1",
        Description = "API REST do sistema de gestão da Melo Barbershop",
        Contact = new OpenApiContact { Name = "Suporte Melobarbershop", Email = "suporte@melobarbershop.com" }
    });

    // Configuração para o Swagger aceitar o token JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT retornado no login. Exemplo: 'eyJhbGci...'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==========================================
// SEED: Migrations + Roles e Admin padrão
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Aplica migrations pendentes automaticamente
    var db = services.GetRequiredService<BarbeariaDbContext>();
    await db.Database.MigrateAsync();

    // Cria roles e usuário admin inicial
    await DbSeeder.SeedAsync(services);
    await SeedDadosBarbearia.PopularAsync(app.Services);
}

// ==========================================
// PIPELINE
// ==========================================

// Swagger sempre habilitado (não só em Development) e como página inicial,
// igual ao SenacFlix — facilita acessar a API direto na raiz.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Melobarbershop API V1");
    c.RoutePrefix = string.Empty; // Swagger como página inicial
});

app.UseHttpsRedirection();

app.UseCors("MelobarbershopCors");

// Autenticação e Autorização devem ser chamados nesta ordem exata
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();