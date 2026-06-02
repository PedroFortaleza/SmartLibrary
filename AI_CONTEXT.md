# 📚 SmartLibrary — AI Context

> Este arquivo serve como contexto completo do projeto para uso com ferramentas de IA (GitHub Copilot, Cursor, ChatGPT, Claude, etc).
> Mantenha-o atualizado conforme o projeto evolui.

---

## 🎯 Visão Geral

**SmartLibrary** é uma plataforma web para gerenciamento de bibliotecas desenvolvida como trabalho final da disciplina **Tópicos III**.

| Item | Valor |
|------|-------|
| Aluno | Pedro Lucas |
| Requisito | #12 — SmartLibrary: Biblioteca Digital Inteligente |
| Stack Back-end | ASP.NET Core 10, C#, SQL Server, Entity Framework Core |
| Stack Front-end | React (responsivo) |
| Autenticação | JWT Bearer Token com Roles |
| Versionamento | Git + EF Core Migrations |

---

## 🏗️ Arquitetura

Clean Architecture com Repository Pattern e Services.

```
SmartLibrary/
├── SmartLibrary.API/             # Controllers, Middlewares, Program.cs, appsettings.json
├── SmartLibrary.Application/     # Services, DTOs, Interfaces, Validators (FluentValidation)
├── SmartLibrary.Domain/          # Entidades, Enums, Domain Events
├── SmartLibrary.Infrastructure/  # DbContext, Repositories, Migrations, Seed, HttpClients
├── SmartLibrary.Web/             # Front-end React ou MVC
└── SmartLibrary.Tests/           # Testes unitários e de integração
```

---

## 👥 Papéis (Roles JWT)

| Role | Descrição | Permissões |
|------|-----------|------------|
| `Administrador` | Gerencia todo o sistema | CRUD completo, relatórios, configurações, multas |
| `Bibliotecario` | Operador do dia a dia | Empréstimos, devoluções, cadastro de livros/exemplares |
| `Aluno` | Usuário final | Consultar acervo, reservar, renovar, avaliar, recomendações |

---

## 🗃️ Entidades (17 no total)

### Usuario
Usuários do sistema com autenticação JWT.
```
Id            int           PK
Nome          varchar(150)  NOT NULL
Email         varchar(200)  UNIQUE NOT NULL
SenhaHash     varchar(512)  NOT NULL
Role          varchar(30)   NOT NULL  -- Administrador | Bibliotecario | Aluno
Ativo         bit           DEFAULT 1
CriadoEm     datetime      NOT NULL
UltimoLogin  datetime      NULL
```

### Aluno
Dados adicionais do perfil de aluno (1:1 com Usuario).
```
Id              int          PK
UsuarioId       int          FK -> Usuario
Matricula       varchar(20)  UNIQUE NOT NULL
Curso           varchar(100) NOT NULL
Turno           varchar(20)  NOT NULL  -- Manha | Tarde | Noite
Cep             varchar(9)   NULL      -- preenchido via ViaCEP
Logradouro      varchar(200) NULL
Cidade          varchar(100) NULL
UF              char(2)      NULL
DataNascimento  date         NULL
Telefone        varchar(20)  NULL
```

### Livro
Catálogo de livros. Dados enriquecidos via Google Books API / OpenLibrary API.
```
Id              int           PK
ISBN            varchar(20)   UNIQUE NOT NULL
Titulo          varchar(300)  NOT NULL
SubTitulo       varchar(300)  NULL
Editora         varchar(150)  NULL
AnoPublicacao   int           NULL
Edicao          varchar(20)   NULL
Idioma          varchar(50)   NULL
NumeroPaginas   int           NULL
Sinopse         text          NULL
CapaUrl         varchar(500)  NULL
GoogleBooksId   varchar(50)   NULL
OpenLibraryId   varchar(50)   NULL
CriadoEm       datetime      NOT NULL
```

### Autor
```
Id            int           PK
Nome          varchar(200)  NOT NULL
Nacionalidade varchar(100)  NULL
Biografia     text          NULL
```

### LivroAutor
Relacionamento N:N entre Livro e Autor.
```
LivroId  int  PK, FK -> Livro
AutorId  int  PK, FK -> Autor
Ordem    int  DEFAULT 1
```

### Categoria
```
Id        int           PK
Nome      varchar(100)  UNIQUE NOT NULL
Descricao varchar(300)  NULL
```

### LivroCategoria
Relacionamento N:N entre Livro e Categoria.
```
LivroId     int  PK, FK -> Livro
CategoriaId int  PK, FK -> Categoria
```

### Exemplar
Cópia física ou digital de um livro no acervo.
```
Id          int          PK
LivroId     int          FK -> Livro
Codigo      varchar(50)  UNIQUE NOT NULL  -- código de tombamento/barras
Localizacao varchar(100) NULL             -- Estante / Prateleira
Tipo        varchar(20)  NOT NULL         -- Fisico | Digital
Estado      varchar(30)  NOT NULL         -- Disponivel | Emprestado | Reservado | Extraviado | Danificado
Ativo       bit          DEFAULT 1
CriadoEm  datetime     NOT NULL
```

### Emprestimo
```
Id                    int          PK
ExemplarId            int          FK -> Exemplar
AlunoId               int          FK -> Aluno
BibliotecarioId       int          FK -> Usuario
DataEmprestimo        datetime     NOT NULL
DataPrevistaDevolucao datetime     NOT NULL
DataDevolucao         datetime     NULL       -- preenchida ao devolver
Status                varchar(30)  NOT NULL   -- Ativo | Devolvido | Atrasado | Renovado
Observacao            varchar(500) NULL
```

### Renovacao
```
Id               int       PK
EmprestimoId     int       FK -> Emprestimo
DataRenovacao    datetime  NOT NULL
NovaDataPrevista datetime  NOT NULL
UsuarioId        int       FK -> Usuario
```

### Reserva
```
Id            int          PK
LivroId       int          FK -> Livro
AlunoId       int          FK -> Aluno
DataReserva   datetime     NOT NULL
DataExpiracao datetime     NOT NULL
Status        varchar(30)  NOT NULL  -- Pendente | Notificado | Retirado | Expirado | Cancelado
NotificadoEm datetime     NULL
```

### Multa
```
Id             int            PK
EmprestimoId   int            FK -> Emprestimo
ValorDiario    decimal(8,2)   NOT NULL
DiasAtraso     int            NOT NULL
ValorTotal     decimal(10,2)  NOT NULL
Status         varchar(20)    NOT NULL  -- Pendente | Pago | Isento
DataPagamento  datetime       NULL
FormaPagamento varchar(50)    NULL      -- Dinheiro | PIX | Cartao
```

### Recomendacao
Sugestões personalizadas geradas pelo sistema.
```
Id         int           PK
AlunoId    int           FK -> Aluno
LivroId    int           FK -> Livro
Score      decimal(5,2)  NOT NULL  -- relevância 0-100
Motivo     varchar(300)  NULL
Visualizada bit          DEFAULT 0
CriadaEm  datetime      NOT NULL
```

### Avaliacao
```
Id          int       PK
LivroId     int       FK -> Livro
AlunoId     int       FK -> Aluno
Nota        int       NOT NULL  -- 1 a 5 estrelas
Comentario  text      NULL
CriadaEm   datetime  NOT NULL
Aprovada    bit       DEFAULT 0  -- moderação do bibliotecário
```

### Notificacao
```
Id         int          PK
UsuarioId  int          FK -> Usuario
Tipo       varchar(50)  NOT NULL  -- Vencimento | Reserva | Multa | Recomendacao
Mensagem   varchar(500) NOT NULL
Lida       bit          DEFAULT 0
CriadaEm  datetime     NOT NULL
```

### ParametroSistema
Configurações gerais administráveis.
```
Id           int           PK
Chave        varchar(100)  UNIQUE NOT NULL
Valor        varchar(300)  NOT NULL
Descricao    varchar(300)  NULL
AtualizadoEm datetime     NOT NULL
```

Parâmetros padrão do Seed:
- `DiasEmprestimo` = `7`
- `ValorMultaDiaria` = `0.50`
- `MaxRenovacoes` = `2`
- `MaxEmprestimosPorAluno` = `3`
- `HorasParaRetiradaReserva` = `48`

### LogAcao
Auditoria de ações críticas.
```
Id         bigint       PK
UsuarioId  int          FK -> Usuario
Acao       varchar(100) NOT NULL  -- ex: EmprestimoRealizado, LivroEditado
Entidade   varchar(50)  NULL
EntidadeId int          NULL
Detalhe    text         NULL      -- JSON com dados antes/depois
CriadoEm  datetime     NOT NULL
```

---

## 🔗 Relacionamentos

```
Usuario       1 : 0..1   Aluno           (todo Aluno tem um Usuario)
Livro         N : N      Autor           (via LivroAutor)
Livro         N : N      Categoria       (via LivroCategoria)
Livro         1 : N      Exemplar
Livro         1 : N      Reserva         (reserva por título)
Exemplar      1 : N      Emprestimo
Aluno         1 : N      Emprestimo
Aluno         1 : N      Reserva
Aluno         1 : N      Recomendacao
Aluno         1 : N      Avaliacao
Emprestimo    1 : N      Renovacao
Emprestimo    1 : 0..1   Multa
Usuario       1 : N      Notificacao
Usuario       1 : N      LogAcao
```

---

## 🌐 API Endpoints

### Públicos (sem autenticação)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/livros` | Lista livros com filtros (título, autor, categoria, ISBN) — paginado |
| GET | `/api/livros/{id}` | Detalhes do livro com autores, categorias e avaliações |
| GET | `/api/livros/{id}/disponibilidade` | Exemplares disponíveis de um livro |
| GET | `/api/livros/isbn/{isbn}` | Busca por ISBN (consulta Google Books se não encontrar localmente) |
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/autores` | Lista autores (paginado) |
| GET | `/api/autores/{id}/livros` | Livros de um autor |
| POST | `/api/auth/login` | Autentica e retorna JWT |
| POST | `/api/auth/registro` | Cria conta de aluno |

### Autenticados (JWT obrigatório)

| Método | Rota | Role Mínima |
|--------|------|-------------|
| GET | `/api/emprestimos` | Todos (Admin/Biblio vê todos; Aluno vê os seus) |
| POST | `/api/emprestimos` | Bibliotecario |
| PUT | `/api/emprestimos/{id}/devolver` | Bibliotecario |
| POST | `/api/emprestimos/{id}/renovar` | Todos |
| GET | `/api/reservas` | Todos |
| POST | `/api/reservas` | Aluno |
| DELETE | `/api/reservas/{id}` | Dono da reserva |
| GET | `/api/multas` | Todos (Admin vê todas; Aluno vê as suas) |
| PUT | `/api/multas/{id}/pagar` | Bibliotecario |
| GET | `/api/recomendacoes` | Aluno |
| POST | `/api/livros` | Bibliotecario |
| PUT | `/api/livros/{id}` | Bibliotecario |
| POST | `/api/exemplares` | Bibliotecario |
| GET | `/api/alunos/{id}/historico` | Bibliotecario |
| POST | `/api/avaliacoes` | Aluno |
| GET | `/api/relatorios/acervo` | Bibliotecario |
| GET | `/api/relatorios/multas` | Administrador |
| GET | `/api/notificacoes` | Todos |

---

## ⚙️ Regras de Negócio

### Empréstimos
- Prazo padrão: **7 dias** (parâmetro `DiasEmprestimo`)
- Aluno com **multa pendente** não pode fazer novo empréstimo
- Máximo de **3 empréstimos simultâneos** por aluno (parâmetro `MaxEmprestimosPorAluno`)
- Máximo de **2 renovações** por empréstimo (parâmetro `MaxRenovacoes`)
- Renovação bloqueada se houver reserva pendente de outro aluno para o mesmo exemplar

### Multas
- Gerada automaticamente ao registrar devolução em atraso
- Valor padrão: **R$ 0,50/dia** (parâmetro `ValorMultaDiaria`)
- `ValorTotal = DiasAtraso × ValorDiario`
- Bibliotecário pode **isentar** multa com justificativa

### Reservas
- Só é possível reservar se **não há exemplares disponíveis**
- Ao ocorrer uma devolução, o sistema **notifica automaticamente** o próximo na fila
- Aluno tem **48 horas** para retirar após notificação (parâmetro `HorasParaRetiradaReserva`), senão a reserva expira

### Recomendações
- Baseadas nas **categorias dos últimos livros** emprestados pelo aluno
- Também consideram livros populares entre alunos do **mesmo curso**
- Score calculado sob demanda ou via job agendado

### Avaliações
- Aluno só pode avaliar livros que **já emprestou**
- Máximo de **1 avaliação por livro por aluno**
- Avaliações ficam pendentes de **aprovação pelo bibliotecário** antes de aparecer publicamente

---

## 🔌 APIs Externas

### Google Books API
- **URL:** `https://www.googleapis.com/books/v1/volumes?q=isbn:{ISBN}`
- **Uso:** Buscar metadados (título, autores, editora, capa, sinopse) ao cadastrar livro por ISBN
- **Campos mapeados:** `volumeInfo.title`, `volumeInfo.authors`, `volumeInfo.publisher`, `volumeInfo.imageLinks.thumbnail`, `volumeInfo.description`

### OpenLibrary API
- **URL:** `https://openlibrary.org/api/books?bibkeys=ISBN:{ISBN}&format=json&jscmd=data`
- **Uso:** Fallback quando Google Books não retorna resultado
- **Configuração:** Não requer chave de API

### ViaCEP
- **URL:** `https://viacep.com.br/ws/{CEP}/json/`
- **Uso:** Preenchimento automático de endereço ao cadastrar aluno
- **Campos mapeados:** `logradouro`, `bairro`, `localidade` → Cidade, `uf` → UF

---

## 🌱 Seed de Dados

| Dado | Detalhes |
|------|----------|
| Admin | `admin@smartlibrary.com` / `Admin@123` — Role: Administrador |
| Bibliotecário | `biblio@smartlibrary.com` / `Biblio@123` — Role: Bibliotecario |
| Categorias | Ficção, Não-Ficção, Tecnologia, Ciências, Direito, História, Filosofia, Literatura, Saúde, Educação |
| ParametroSistema | DiasEmprestimo=7, ValorMultaDiaria=0.50, MaxRenovacoes=2, MaxEmprestimosPorAluno=3 |
| Livros | 5 livros com autores, categorias e exemplares |

---

## 📦 Padrões de Código

### Nomeclatura
- **Entidades:** PascalCase singular (`Emprestimo`, `Livro`)
- **DTOs:** sufixo `Dto`, `CreateDto`, `UpdateDto` (`LivroDto`, `CreateEmprestimoDto`)
- **Services:** sufixo `Service` (`EmprestimoService`)
- **Repositories:** sufixo `Repository` (`LivroRepository`)
- **Controllers:** sufixo `Controller`, rota plural kebab-case (`/api/emprestimos`)

### Respostas da API
```json
// Sucesso
{ "data": { ... }, "message": "Operação realizada com sucesso" }

// Erro de validação (400)
{ "errors": { "campo": ["mensagem"] } }

// Erro de negócio (422)
{ "message": "Aluno possui multa pendente e não pode realizar empréstimo." }

// Não encontrado (404)
{ "message": "Livro não encontrado." }
```

### Paginação
```json
// Query params: ?page=1&pageSize=10&search=&categoriaId=
{
  "data": [ ... ],
  "totalItems": 100,
  "page": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

---

## ✅ Checklist de Requisitos da Disciplina

- [x] Back-end ASP.NET Core 10
- [x] Banco SQL Server
- [x] Entity Framework Core + Migrations
- [x] Seed de dados
- [x] Mínimo 17 entidades → **17 implementadas**
- [x] API autenticada (mín. 3) → **18 endpoints autenticados**
- [x] API pública (mín. 3) → **9 endpoints públicos**
- [x] JWT com Roles
- [x] Consumo de API de terceiros (Google Books + OpenLibrary + ViaCEP)
- [ ] Front-end responsivo
- [x] Controle de versão (Git)
- [x] Controle de versão de banco (EF Migrations)
- [ ] Diagrama de banco de dados
- [ ] Diagrama de casos de uso
- [ ] Documento de escopo
- [ ] Apresentação final