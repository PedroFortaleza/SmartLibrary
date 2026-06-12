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
        // Remove livros sem imagem de capa (cascade apaga LivroAutores, LivroCategorias, Exemplares)
        var semCapa = await context.Livros
            .Where(l => l.CapaUrl == null || l.CapaUrl == "")
            .ToListAsync();

        if (semCapa.Count > 0)
        {
            context.Livros.RemoveRange(semCapa);
            await context.SaveChangesAsync();

            var orfaos = await context.Autores
                .Where(a => !a.LivroAutores.Any())
                .ToListAsync();

            if (orfaos.Count > 0)
            {
                context.Autores.RemoveRange(orfaos);
                await context.SaveChangesAsync();
            }
        }

        var dados = GetDadosLivros();
        var isbnsExistentes = (await context.Livros.Select(l => l.ISBN).ToListAsync()).ToHashSet();
        var novos = dados.Where(d => !isbnsExistentes.Contains(d.ISBN)).ToList();

        if (novos.Count == 0) return;

        var cats = await context.Categorias.ToDictionaryAsync(c => c.Nome);

        var todosNomes = novos.SelectMany(n => n.Autores).Distinct().ToHashSet();
        var autoresExistentes = await context.Autores
            .Where(a => todosNomes.Contains(a.Nome))
            .ToDictionaryAsync(a => a.Nome);

        var autoresPorNome = new Dictionary<string, Autor>(autoresExistentes);
        foreach (var nome in todosNomes.Where(n => !autoresExistentes.ContainsKey(n)))
        {
            var autor = new Autor { Nome = nome };
            context.Autores.Add(autor);
            autoresPorNome[nome] = autor;
        }
        await context.SaveChangesAsync();

        var livros = novos.Select(d => new Livro
        {
            ISBN          = d.ISBN,
            Titulo        = d.Titulo,
            SubTitulo     = d.SubTitulo,
            Editora       = d.Editora,
            AnoPublicacao = d.AnoPublicacao,
            Idioma        = d.Idioma,
            NumeroPaginas = d.NumeroPaginas,
            CapaUrl       = d.CapaUrl,
            CriadoEm     = DateTime.UtcNow,
            LivroAutores  = d.Autores.Select((nome, i) => new LivroAutor
            {
                AutorId = autoresPorNome[nome].Id,
                Ordem   = i + 1
            }).ToList(),
            LiveCategorias = [ new() { CategoriaId = cats[d.Categoria].Id } ],
            Exemplares = d.ComExemplares
                ? [
                    new() { Codigo = $"{d.ISBN}-F01", Tipo = TipoExemplar.Fisico,  Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow },
                    new() { Codigo = $"{d.ISBN}-D01", Tipo = TipoExemplar.Digital, Estado = EstadoExemplar.Disponivel, CriadoEm = DateTime.UtcNow }
                  ]
                : []
        }).ToList();

        context.Livros.AddRange(livros);
        await context.SaveChangesAsync();
    }

    private record DadosLivro(
        string ISBN, string Titulo, string? SubTitulo, string Editora,
        int AnoPublicacao, string Idioma, int NumeroPaginas, string CapaUrl,
        string[] Autores, string Categoria, bool ComExemplares = true);

    private static List<DadosLivro> GetDadosLivros() =>
    [
        // ── Tecnologia ───────────────────────────────────────────────────────
        new("9780132350884", "Clean Code", "A Handbook of Agile Software Craftsmanship",
            "Prentice Hall", 2008, "Inglês", 431,
            "https://covers.openlibrary.org/b/isbn/9780132350884-L.jpg",
            ["Robert C. Martin"], "Tecnologia"),

        new("9780201616224", "The Pragmatic Programmer", "From Journeyman to Master",
            "Addison-Wesley", 1999, "Inglês", 352,
            "https://covers.openlibrary.org/b/isbn/9780201616224-L.jpg",
            ["David Thomas", "Andrew Hunt"], "Tecnologia"),

        new("9780201633610", "Design Patterns", "Elements of Reusable Object-Oriented Software",
            "Addison-Wesley", 1994, "Inglês", 395,
            "https://covers.openlibrary.org/b/isbn/9780201633610-L.jpg",
            ["Erich Gamma", "Richard Helm", "Ralph Johnson", "John Vlissides"], "Tecnologia"),

        // ── Ficção ───────────────────────────────────────────────────────────
        new("9780451524935", "1984", null,
            "Signet Classic", 1949, "Inglês", 328,
            "https://covers.openlibrary.org/b/isbn/9780451524935-L.jpg",
            ["George Orwell"], "Ficção"),

        new("9780618640157", "O Senhor dos Anéis", null,
            "Houghton Mifflin", 1954, "Português", 1178,
            "https://covers.openlibrary.org/b/isbn/9780618640157-L.jpg",
            ["J.R.R. Tolkien"], "Ficção"),

        new("9780060850524", "Admirável Mundo Novo", null,
            "Harper Perennial", 1932, "Português", 311,
            "https://covers.openlibrary.org/b/isbn/9780060850524-L.jpg",
            ["Aldous Huxley"], "Ficção"),

        // ── Literatura ───────────────────────────────────────────────────────
        new("9780060883287", "Cem Anos de Solidão", null,
            "Harper Perennial", 1967, "Português", 417,
            "https://covers.openlibrary.org/b/isbn/9780060883287-L.jpg",
            ["Gabriel García Márquez"], "Literatura"),

        new("9780743273565", "O Grande Gatsby", null,
            "Scribner", 1925, "Português", 180,
            "https://covers.openlibrary.org/b/isbn/9780743273565-L.jpg",
            ["F. Scott Fitzgerald"], "Literatura"),

        new("9788535902976", "Dom Casmurro", null,
            "Companhia das Letras", 1899, "Português", 279,
            "https://covers.openlibrary.org/b/isbn/9788535902976-L.jpg",
            ["Machado de Assis"], "Literatura"),

        // ── Filosofia ────────────────────────────────────────────────────────
        new("9780140449181", "A República", null,
            "Penguin Classics", -380, "Português", 416,
            "https://covers.openlibrary.org/b/isbn/9780140449181-L.jpg",
            ["Platão"], "Filosofia"),

        new("9780140441185", "Assim Falou Zaratustra", null,
            "Penguin Classics", 1883, "Português", 368,
            "https://covers.openlibrary.org/b/isbn/9780140441185-L.jpg",
            ["Friedrich Nietzsche"], "Filosofia"),

        // ── História ─────────────────────────────────────────────────────────
        new("9780062316097", "Sapiens", "Uma Breve História da Humanidade",
            "Harper", 2011, "Português", 443,
            "https://covers.openlibrary.org/b/isbn/9780062316097-L.jpg",
            ["Yuval Noah Harari"], "História"),

        new("9780525512172", "21 Lições para o Século 21", null,
            "Spiegel & Grau", 2018, "Português", 372,
            "https://covers.openlibrary.org/b/isbn/9780525512172-L.jpg",
            ["Yuval Noah Harari"], "História"),

        // ── Ciências ─────────────────────────────────────────────────────────
        new("9780553380163", "Uma Breve História do Tempo", null,
            "Bantam Books", 1988, "Português", 212,
            "https://covers.openlibrary.org/b/isbn/9780553380163-L.jpg",
            ["Stephen Hawking"], "Ciências"),

        new("9780198788607", "O Gene Egoísta", null,
            "Oxford University Press", 1976, "Português", 360,
            "https://covers.openlibrary.org/b/isbn/9780198788607-L.jpg",
            ["Richard Dawkins"], "Ciências"),

        // ── Saúde ────────────────────────────────────────────────────────────
        new("9781501144325", "Por Que Dormimos", "Unlocking the Power of Sleep and Dreams",
            "Scribner", 2017, "Português", 368,
            "https://covers.openlibrary.org/b/isbn/9781501144325-L.jpg",
            ["Matthew Walker"], "Saúde"),

        new("9780345472328", "Mindset", "A Nova Psicologia do Sucesso",
            "Ballantine Books", 2006, "Português", 276,
            "https://covers.openlibrary.org/b/isbn/9780345472328-L.jpg",
            ["Carol S. Dweck"], "Saúde"),

        // ── Educação ─────────────────────────────────────────────────────────
        new("9780062852687", "Ultralearning", "Master Hard Skills, Outsmart the Competition",
            "HarperBusiness", 2019, "Inglês", 304,
            "https://covers.openlibrary.org/b/isbn/9780062852687-L.jpg",
            ["Scott Young"], "Educação"),

        // ── Direito ──────────────────────────────────────────────────────────
        new("9780141034539", "A Regra do Direito", null,
            "Penguin Books", 2010, "Português", 256,
            "https://covers.openlibrary.org/b/isbn/9780141034539-L.jpg",
            ["Tom Bingham"], "Direito"),

        // ── Não-Ficção ───────────────────────────────────────────────────────
        new("9780812981605", "O Poder do Hábito", "Por que fazemos o que fazemos na vida e nos negócios",
            "Random House", 2012, "Português", 371,
            "https://covers.openlibrary.org/b/isbn/9780812981605-L.jpg",
            ["Charles Duhigg"], "Não-Ficção"),

        // ── Sem exemplares ───────────────────────────────────────────────────
        new("9780201485677", "Refactoring", "Improving the Design of Existing Code",
            "Addison-Wesley", 1999, "Inglês", 448,
            "https://covers.openlibrary.org/b/isbn/9780201485677-L.jpg",
            ["Martin Fowler"], "Tecnologia", ComExemplares: false),

        new("9780201835953", "The Mythical Man-Month", "Essays on Software Engineering",
            "Addison-Wesley", 1995, "Inglês", 336,
            "https://covers.openlibrary.org/b/isbn/9780201835953-L.jpg",
            ["Frederick P. Brooks Jr."], "Tecnologia", ComExemplares: false),

        new("9780735619678", "Code Complete", "A Practical Handbook of Software Construction",
            "Microsoft Press", 2004, "Inglês", 960,
            "https://covers.openlibrary.org/b/isbn/9780735619678-L.jpg",
            ["Steve McConnell"], "Tecnologia", ComExemplares: false),

        new("9780262033848", "Introduction to Algorithms", null,
            "MIT Press", 2009, "Inglês", 1292,
            "https://covers.openlibrary.org/b/isbn/9780262033848-L.jpg",
            ["Thomas H. Cormen", "Charles E. Leiserson", "Ronald L. Rivest", "Clifford Stein"], "Tecnologia", ComExemplares: false),

        new("9780805210408", "O Processo", null,
            "Schocken Books", 1925, "Português", 255,
            "https://covers.openlibrary.org/b/isbn/9780805210408-L.jpg",
            ["Franz Kafka"], "Ficção", ComExemplares: false),

        new("9781451673319", "Fahrenheit 451", null,
            "Simon & Schuster", 1953, "Português", 249,
            "https://covers.openlibrary.org/b/isbn/9781451673319-L.jpg",
            ["Ray Bradbury"], "Ficção", ComExemplares: false),

        new("9780547928227", "O Hobbit", null,
            "Houghton Mifflin Harcourt", 1937, "Português", 310,
            "https://covers.openlibrary.org/b/isbn/9780547928227-L.jpg",
            ["J.R.R. Tolkien"], "Ficção", ComExemplares: false),

        new("9780486415871", "Crime e Castigo", null,
            "Dover Publications", 1866, "Português", 576,
            "https://covers.openlibrary.org/b/isbn/9780486415871-L.jpg",
            ["Fiódor Dostoiévski"], "Literatura", ComExemplares: false),

        new("9780316769174", "O Apanhador no Campo de Centeio", null,
            "Little, Brown and Company", 1951, "Português", 277,
            "https://covers.openlibrary.org/b/isbn/9780316769174-L.jpg",
            ["J.D. Salinger"], "Literatura", ComExemplares: false),

        new("9780553213690", "A Metamorfose", null,
            "Bantam Classics", 1915, "Português", 96,
            "https://covers.openlibrary.org/b/isbn/9780553213690-L.jpg",
            ["Franz Kafka"], "Literatura", ComExemplares: false),

        new("9780521657297", "Crítica da Razão Pura", null,
            "Cambridge University Press", 1781, "Português", 785,
            "https://covers.openlibrary.org/b/isbn/9780521657297-L.jpg",
            ["Immanuel Kant"], "Filosofia", ComExemplares: false),

        new("9780486217628", "O Mundo como Vontade e Representação", null,
            "Dover Publications", 1818, "Português", 534,
            "https://covers.openlibrary.org/b/isbn/9780486217628-L.jpg",
            ["Arthur Schopenhauer"], "Filosofia", ComExemplares: false),

        new("9780062464347", "Homo Deus", "Uma Breve História do Amanhã",
            "Harper", 2015, "Português", 450,
            "https://covers.openlibrary.org/b/isbn/9780062464347-L.jpg",
            ["Yuval Noah Harari"], "História", ComExemplares: false),

        new("9781599869773", "A Arte da Guerra", null,
            "Filiquarian Publishing", 500, "Português", 68,
            "https://covers.openlibrary.org/b/isbn/9781599869773-L.jpg",
            ["Sun Tzu"], "História", ComExemplares: false),

        new("9780140432053", "A Origem das Espécies", null,
            "Penguin Classics", 1859, "Português", 480,
            "https://covers.openlibrary.org/b/isbn/9780140432053-L.jpg",
            ["Charles Darwin"], "Ciências", ComExemplares: false),

        new("9780553802023", "O Universo Numa Casca de Noz", null,
            "Bantam Books", 2001, "Português", 224,
            "https://covers.openlibrary.org/b/isbn/9780553802023-L.jpg",
            ["Stephen Hawking"], "Ciências", ComExemplares: false),

        new("9780553383713", "Inteligência Emocional", null,
            "Bantam Books", 1995, "Português", 352,
            "https://covers.openlibrary.org/b/isbn/9780553383713-L.jpg",
            ["Daniel Goleman"], "Saúde", ComExemplares: false),

        new("9780671027032", "Como Fazer Amigos e Influenciar Pessoas", null,
            "Simon & Schuster", 1936, "Português", 291,
            "https://covers.openlibrary.org/b/isbn/9780671027032-L.jpg",
            ["Dale Carnegie"], "Educação", ComExemplares: false),

        new("9780374533557", "Rápido e Devagar", "Duas Formas de Pensar",
            "Farrar, Straus and Giroux", 2011, "Português", 499,
            "https://covers.openlibrary.org/b/isbn/9780374533557-L.jpg",
            ["Daniel Kahneman"], "Não-Ficção", ComExemplares: false),

        new("9780140442014", "O Contrato Social", null,
            "Penguin Classics", 1762, "Português", 186,
            "https://covers.openlibrary.org/b/isbn/9780140442014-L.jpg",
            ["Jean-Jacques Rousseau"], "Direito", ComExemplares: false),
    ];
}
