using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SmartLibrary.API.Middleware;
using SmartLibrary.Application.Interfaces.External;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Application.Interfaces.Services;
using SmartLibrary.Application.Services;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infrastructure.Data;
using SmartLibrary.Infrastructure.Data.Seed;
using SmartLibrary.Infrastructure.HttpClients;
using SmartLibrary.Infrastructure.Repositories;
using SmartLibrary.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ── DbContext ───────────────────────────────────────────────────────────────
builder.Services.AddDbContext<SmartLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── JWT ─────────────────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

// ── Repositories ────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IMultaRepository, MultaRepository>();
builder.Services.AddScoped<IParametroRepository, ParametroRepository>();
builder.Services.AddScoped<IExemplarRepository, ExemplarRepository>();
builder.Services.AddScoped<IBaseRepository<Avaliacao>, BaseRepository<Avaliacao>>();
builder.Services.AddScoped<IBaseRepository<Recomendacao>, BaseRepository<Recomendacao>>();
builder.Services.AddScoped<IBaseRepository<Notificacao>, BaseRepository<Notificacao>>();
builder.Services.AddScoped<IBaseRepository<Autor>, BaseRepository<Autor>>();
builder.Services.AddScoped<IBaseRepository<Categoria>, BaseRepository<Categoria>>();
builder.Services.AddScoped<IBaseRepository<Livro>, BaseRepository<Livro>>();

// ── Application Services ────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IMultaService, MultaService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IRecomendacaoService, RecomendacaoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
builder.Services.AddScoped<IExemplarService, ExemplarService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IParametroService, ParametroService>();

// ── External HTTP Clients ───────────────────────────────────────────────────
builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksClient>();
builder.Services.AddHttpClient<IOpenLibraryService, OpenLibraryClient>();
builder.Services.AddHttpClient<IViaCepService, ViaCepClient>();

// ── OpenAPI + Scalar ────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// ── Middleware Pipeline ─────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "SmartLibrary API";
    options.AddPreferredSecuritySchemes("Bearer");
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ── Seed ────────────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SmartLibraryDbContext>();
    await DataSeeder.SeedAsync(context);
}

app.Run();
