using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(SmartLibraryDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedUsuariosAsync(context);
        await SeedCategoriasAsync(context);
        await SeedParametrosAsync(context);
        await SeedLivrosAsync(context);
    }

    private static async Task SeedUsuariosAsync(SmartLibraryDbContext context)
    {
        if (await context.Usuarios.AnyAsync()) return;

        var usuarios = new List<Usuario>
        {
            new()
            {
                Nome = "Administrador",
                Email = "admin@smartlibrary.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = RoleUsuario.Administrador,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            },
            new()
            {
                Nome = "Bibliotecário",
                Email = "biblio@smartlibrary.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Biblio@123"),
                Role = RoleUsuario.Bibliotecario,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            }
        };

        context.Usuarios.AddRange(usuarios);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriasAsync(SmartLibraryDbContext context)
    {
        if (await context.Categorias.AnyAsync()) return;

        var categorias = new[]
        {
            "Ficção", "Não-Ficção", "Tecnologia", "Ciências", "Direito",
            "História", "Filosofia", "Literatura", "Saúde", "Educação"
        }.Select(nome => new Categoria { Nome = nome }).ToList();

        context.Categorias.AddRange(categorias);
        await context.SaveChangesAsync();
    }

    private static async Task SeedParametrosAsync(SmartLibraryDbContext context)
    {
        if (await context.ParametrosSistema.AnyAsync()) return;

        var parametros = new List<ParametroSistema>
        {
            new() { Chave = "DiasEmprestimo",           Valor = "7",    Descricao = "Prazo padrão de empréstimo em dias",              AtualizadoEm = DateTime.UtcNow },
            new() { Chave = "ValorMultaDiaria",          Valor = "0.50", Descricao = "Valor da multa por dia de atraso",                AtualizadoEm = DateTime.UtcNow },
            new() { Chave = "MaxRenovacoes",             Valor = "2",    Descricao = "Número máximo de renovações por empréstimo",      AtualizadoEm = DateTime.UtcNow },
            new() { Chave = "MaxEmprestimosPorAluno",    Valor = "3",    Descricao = "Número máximo de empréstimos simultâneos",        AtualizadoEm = DateTime.UtcNow },
            new() { Chave = "HorasParaRetiradaReserva",  Valor = "48",   Descricao = "Horas para retirar o livro após notificação",     AtualizadoEm = DateTime.UtcNow }
        };

        context.ParametrosSistema.AddRange(parametros);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLivrosAsync(SmartLibraryDbContext context)
    {
        if (await context.Livros.AnyAsync()) return;

        var catTecnologia = await context.Categorias.FirstAsync(c => c.Nome == "Tecnologia");
        var catFiccao     = await context.Categorias.FirstAsync(c => c.Nome == "Ficção");
        var catFilosofia  = await context.Categorias.FirstAsync(c => c.Nome == "Filosofia");
        var catHistoria   = await context.Categorias.FirstAsync(c => c.Nome == "História");
        var catLiteratura = await context.Categorias.FirstAsync(c => c.Nome == "Literatura");

        var autores = new List<Autor>
        {
            new() { Nome = "Robert C. Martin",    Nacionalidade = "Americana" },
            new() { Nome = "George Orwell",        Nacionalidade = "Britânica" },
            new() { Nome = "Plato",                Nacionalidade = "Grega"    },
            new() { Nome = "Yuval Noah Harari",    Nacionalidade = "Israelense" },
            new() { Nome = "Gabriel García Márquez", Nacionalidade = "Colombiana" }
        };

        context.Autores.AddRange(autores);
        await context.SaveChangesAsync();

        var livros = new List<Livro>
        {
            new()
            {
                ISBN = "9780132350884", Titulo = "Clean Code",
                Editora = "Prentice Hall", AnoPublicacao = 2008, Idioma = "Inglês",
                NumeroPaginas = 431, CriadoEm = DateTime.UtcNow,
                LivroAutores = [ new() { AutorId = autores[0].Id, Ordem = 1 } ],
                LiveCategorias = [ new() { CategoriaId = catTecnologia.Id } ],
                Exemplares = [
                    new() { Codigo = "TECH-001", Tipo = TipoExemplar.Fisico,  Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow },
                    new() { Codigo = "TECH-002", Tipo = TipoExemplar.Digital, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                ]
            },
            new()
            {
                ISBN = "9780451524935", Titulo = "1984",
                Editora = "Signet Classic", AnoPublicacao = 1949, Idioma = "Inglês",
                NumeroPaginas = 328, CriadoEm = DateTime.UtcNow,
                LivroAutores = [ new() { AutorId = autores[1].Id, Ordem = 1 } ],
                LiveCategorias = [ new() { CategoriaId = catFiccao.Id } ],
                Exemplares = [
                    new() { Codigo = "FIC-001", Tipo = TipoExemplar.Fisico, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow },
                    new() { Codigo = "FIC-002", Tipo = TipoExemplar.Fisico, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                ]
            },
            new()
            {
                ISBN = "9780140449181", Titulo = "A República",
                Editora = "Penguin Classics", AnoPublicacao = -380, Idioma = "Português",
                NumeroPaginas = 416, CriadoEm = DateTime.UtcNow,
                LivroAutores = [ new() { AutorId = autores[2].Id, Ordem = 1 } ],
                LiveCategorias = [ new() { CategoriaId = catFilosofia.Id } ],
                Exemplares = [
                    new() { Codigo = "FIL-001", Tipo = TipoExemplar.Fisico, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                ]
            },
            new()
            {
                ISBN = "9780062316097", Titulo = "Sapiens",
                SubTitulo = "Uma breve história da humanidade",
                Editora = "Harper", AnoPublicacao = 2011, Idioma = "Português",
                NumeroPaginas = 443, CriadoEm = DateTime.UtcNow,
                LivroAutores = [ new() { AutorId = autores[3].Id, Ordem = 1 } ],
                LiveCategorias = [ new() { CategoriaId = catHistoria.Id } ],
                Exemplares = [
                    new() { Codigo = "HIS-001", Tipo = TipoExemplar.Fisico,  Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow },
                    new() { Codigo = "HIS-002", Tipo = TipoExemplar.Digital, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                ]
            },
            new()
            {
                ISBN = "9780060883287", Titulo = "Cem Anos de Solidão",
                Editora = "Harper Perennial", AnoPublicacao = 1967, Idioma = "Português",
                NumeroPaginas = 417, CriadoEm = DateTime.UtcNow,
                LivroAutores = [ new() { AutorId = autores[4].Id, Ordem = 1 } ],
                LiveCategorias = [ new() { CategoriaId = catLiteratura.Id } ],
                Exemplares = [
                    new() { Codigo = "LIT-001", Tipo = TipoExemplar.Fisico, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow },
                    new() { Codigo = "LIT-002", Tipo = TipoExemplar.Fisico, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                ]
            }
        };

        context.Livros.AddRange(livros);
        await context.SaveChangesAsync();
    }
}
