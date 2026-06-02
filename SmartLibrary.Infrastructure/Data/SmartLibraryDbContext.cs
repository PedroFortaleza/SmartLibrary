using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infrastructure.Data.Configurations;

namespace SmartLibrary.Infrastructure.Data;

public class SmartLibraryDbContext(DbContextOptions<SmartLibraryDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<LivroAutor> LivroAutores => Set<LivroAutor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<LivroCategoria> LiveCategorias => Set<LivroCategoria>();
    public DbSet<Exemplar> Exemplares => Set<Exemplar>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Renovacao> Renovacoes => Set<Renovacao>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Multa> Multas => Set<Multa>();
    public DbSet<Recomendacao> Recomendacoes => Set<Recomendacao>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();
    public DbSet<ParametroSistema> ParametrosSistema => Set<ParametroSistema>();
    public DbSet<LogAcao> LogAcoes => Set<LogAcao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new AlunoConfiguration());
        modelBuilder.ApplyConfiguration(new LivroConfiguration());
        modelBuilder.ApplyConfiguration(new AutorConfiguration());
        modelBuilder.ApplyConfiguration(new LivroAutorConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new LivroCategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new ExemplarConfiguration());
        modelBuilder.ApplyConfiguration(new EmprestimoConfiguration());
        modelBuilder.ApplyConfiguration(new RenovacaoConfiguration());
        modelBuilder.ApplyConfiguration(new ReservaConfiguration());
        modelBuilder.ApplyConfiguration(new MultaConfiguration());
        modelBuilder.ApplyConfiguration(new RecomendacaoConfiguration());
        modelBuilder.ApplyConfiguration(new AvaliacaoConfiguration());
        modelBuilder.ApplyConfiguration(new NotificacaoConfiguration());
        modelBuilder.ApplyConfiguration(new ParametroSistemaConfiguration());
        modelBuilder.ApplyConfiguration(new LogAcaoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
