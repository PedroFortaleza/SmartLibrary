using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nacionalidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Biografia = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SubTitulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Editora = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AnoPublicacao = table.Column<int>(type: "int", nullable: true),
                    Edicao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Idioma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroPaginas = table.Column<int>(type: "int", nullable: true),
                    Sinopse = table.Column<string>(type: "text", nullable: true),
                    CapaUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GoogleBooksId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OpenLibraryId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametrosSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exemplares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exemplares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exemplares_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiveCategorias",
                columns: table => new
                {
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveCategorias", x => new { x.LivroId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_LiveCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiveCategorias_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivroAutores",
                columns: table => new
                {
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAutores", x => new { x.LivroId, x.AutorId });
                    table.ForeignKey(
                        name: "FK_LivroAutores_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroAutores_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Curso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UF = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: true),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alunos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogAcoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Entidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntidadeId = table.Column<int>(type: "int", nullable: true),
                    Detalhe = table.Column<string>(type: "text", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAcoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogAcoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Lida = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: true),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aprovada = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExemplarId = table.Column<int>(type: "int", nullable: false),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    BibliotecarioId = table.Column<int>(type: "int", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPrevistaDevolucao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Exemplares_ExemplarId",
                        column: x => x.ExemplarId,
                        principalTable: "Exemplares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Usuarios_BibliotecarioId",
                        column: x => x.BibliotecarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recomendacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Visualizada = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CriadaEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recomendacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recomendacoes_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recomendacoes_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    DataReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NotificadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Multas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmprestimoId = table.Column<int>(type: "int", nullable: false),
                    ValorDiario = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DiasAtraso = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormaPagamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Multas_Emprestimos_EmprestimoId",
                        column: x => x.EmprestimoId,
                        principalTable: "Emprestimos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Renovacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmprestimoId = table.Column<int>(type: "int", nullable: false),
                    DataRenovacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NovaDataPrevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renovacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Renovacoes_Emprestimos_EmprestimoId",
                        column: x => x.EmprestimoId,
                        principalTable: "Emprestimos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Renovacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Matricula",
                table: "Alunos",
                column: "Matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_UsuarioId",
                table: "Alunos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_AlunoId",
                table: "Avaliacoes",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_LivroId_AlunoId",
                table: "Avaliacoes",
                columns: new[] { "LivroId", "AlunoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome",
                table: "Categorias",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_AlunoId",
                table: "Emprestimos",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_BibliotecarioId",
                table: "Emprestimos",
                column: "BibliotecarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_ExemplarId",
                table: "Emprestimos",
                column: "ExemplarId");

            migrationBuilder.CreateIndex(
                name: "IX_Exemplares_Codigo",
                table: "Exemplares",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exemplares_LivroId",
                table: "Exemplares",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveCategorias_CategoriaId",
                table: "LiveCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutores_AutorId",
                table: "LivroAutores",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_ISBN",
                table: "Livros",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogAcoes_UsuarioId",
                table: "LogAcoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Multas_EmprestimoId",
                table: "Multas",
                column: "EmprestimoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UsuarioId",
                table: "Notificacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosSistema_Chave",
                table: "ParametrosSistema",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recomendacoes_AlunoId",
                table: "Recomendacoes",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Recomendacoes_LivroId",
                table: "Recomendacoes",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Renovacoes_EmprestimoId",
                table: "Renovacoes",
                column: "EmprestimoId");

            migrationBuilder.CreateIndex(
                name: "IX_Renovacoes_UsuarioId",
                table: "Renovacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_AlunoId",
                table: "Reservas",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_LivroId",
                table: "Reservas",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "LiveCategorias");

            migrationBuilder.DropTable(
                name: "LivroAutores");

            migrationBuilder.DropTable(
                name: "LogAcoes");

            migrationBuilder.DropTable(
                name: "Multas");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "ParametrosSistema");

            migrationBuilder.DropTable(
                name: "Recomendacoes");

            migrationBuilder.DropTable(
                name: "Renovacoes");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Exemplares");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
