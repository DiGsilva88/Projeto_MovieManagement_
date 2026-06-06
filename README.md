# 🎬 MovieManagement

Aplicação de gestão de filmes desenvolvida em C# com arquitetura em camadas, interfaces, regras de negócio e persistência de dados.

---

## 📁 Estrutura do Projeto

```
MovieManagement/
├── Domain/                              # Entidades e Interfaces
│   ├── Entidades/
│   │   ├── Filme.cs
│   │   ├── Categoria.cs
│   │   └── Realizador.cs
│   └── Interfaces/
│       ├── IRepository.cs               # Interface genérica (base)
│       ├── IFilmeRepository.cs
│       ├── ICategoriaRepository.cs
│       └── IRealizadorRepository.cs
│
├── Business/                            # Regras de Negócio
│   └── Servicos/
│       ├── BaseService.cs               # Serviço base genérico
│       ├── FilmeService.cs
│       ├── CategoriaService.cs
│       └── RealizadorService.cs
│
├── Data/                                # Persistência de Dados
│   └── Repositorio/
│       ├── Memory/                      # Persistência em memória
│       │   ├── FilmeRepositoryMemory.cs
│       │   ├── CategoriaRepositoryMemory.cs
│       │   └── RealizadorRepositoryMemory.cs
│       └── SQLite/                      # Persistência em SQLite
│           ├── DatabaseInitializer.cs
│           ├── FilmeRepositorySQLite.cs
│           ├── CategoriaRepositorySQLite.cs
│           └── RealizadorRepositorySQLite.cs
│
└── GestaoFilme.appConsola/              # Interface com o Utilizador (UI)
    └── Program.cs
```

---

## 🏗️ Arquitetura

O projeto segue uma **arquitetura em camadas**, onde cada camada tem uma responsabilidade bem definida:

| Camada | Projeto | Responsabilidade |
|--------|---------|-----------------|
| **UI** | `GestaoFilme.appConsola` | Menus, inputs e apresentação |
| **Business** | `Business` | Regras de negócio e validações |
| **Data** | `Data` | Persistência (memória ou SQLite) |
| **Domain** | `Domain` | Entidades e contratos (interfaces) |

### Dependências entre camadas

```
UI  →  Business  →  Domain
              Data  →  Domain
UI  →  Data (apenas para instanciar os repositórios)
```

> **Importante:** A Business nunca referencia a Data diretamente. Os repositórios são injetados via construtor (Injeção de Dependência).

---

## ✨ Funcionalidades

### Parte 1 — Filmes
- ✅ Adicionar filme
- ✅ Listar filmes
- ✅ Procurar filme por título
- ✅ Remover filme

### Parte 2 — Categorias e Realizadores
- ✅ Adicionar / Listar / Procurar / Remover categorias
- ✅ Adicionar / Listar / Procurar / Remover realizadores

### Parte 3 — Relações + SQLite
- ✅ Cada filme está associado a uma Categoria e a um Realizador
- ✅ Validação das relações na Business Layer antes de adicionar filmes
- ✅ Persistência em SQLite com tabelas e chaves estrangeiras
- ✅ Persistência em memória mantida e permutável sem alterar UI, Business ou Domain

---

## 🗂️ Entidades

```csharp
// Filme (Parte 3 — inclui relações)
public class Filme
{
    public int    Id            { get; set; }
    public string Titulo        { get; set; }
    public int    Ano           { get; set; }
    public string Lingua        { get; set; }
    public double Classificacao { get; set; }
    public int    CategoriaId   { get; set; }  // Parte 3
    public int    RealizadorId  { get; set; }  // Parte 3
}

// Categoria
public class Categoria
{
    public int    Id   { get; set; }
    public string Nome { get; set; }
}

// Realizador
public class Realizador
{
    public int    Id   { get; set; }
    public string Nome { get; set; }
    public string Pais { get; set; }
}
```

---

## ✅ Regras de Negócio

### Filme
- Título **obrigatório**
- Não podem existir **títulos duplicados**
- Classificação deve estar **entre 0 e 5**
- A **categoria indicada deve existir** antes de adicionar o filme
- O **realizador indicado deve existir** antes de adicionar o filme

### Categoria
- Nome **obrigatório**
- Não podem existir **categorias duplicadas**

### Realizador
- Nome **obrigatório**
- País **obrigatório**
- Não podem existir **realizadores duplicados**

---

## 🔌 Persistência

A arquitetura permite trocar a persistência **sem alterar UI, Business ou Domain**.  
A única alteração necessária é no bloco de configuração no topo do `Program.cs`:

```csharp
// --- SQLite (Parte 3 — ativo) ---
IFilmeRepository      filmeRepo      = new FilmeRepositorySQLite(connectionString);
ICategoriaRepository  categoriaRepo  = new CategoriaRepositorySQLite(connectionString);
IRealizadorRepository realizadorRepo = new RealizadorRepositorySQLite(connectionString);

// --- Memória (descomentar para testar sem base de dados) ---
// IFilmeRepository      filmeRepo      = new FilmeRepositoryMemory();
// ICategoriaRepository  categoriaRepo  = new CategoriaRepositoryMemory();
// IRealizadorRepository realizadorRepo = new RealizadorRepositoryMemory();
```

| Fase | Tipo | Classes |
|------|------|---------|
| Partes 1 e 2 | Memória (`List<T>`) | `*RepositoryMemory` |
| Parte 3 | SQLite | `*RepositorySQLite` |

---

## 🧱 Padrões de Reutilização

### `IRepository<T>` — interface genérica
Elimina a duplicação de métodos entre as três interfaces de repositório. Cada interface específica apenas declara o que é exclusivo:

```csharp
public interface IRepository<T>
{
    void           Adicionar(T entidade);
    IEnumerable<T> ListarTodos();
    T?             ObterPorId(int id);
    void           Remover(int id);
}

public interface IFilmeRepository : IRepository<Filme>
{
    Filme? ObterPorTitulo(string titulo);
}
```

### `BaseService<T>` — serviço base genérico
Contém as operações comuns (`ListarTodos`, `ObterPorId`, `Remover`) para evitar duplicação entre `FilmeService`, `CategoriaService` e `RealizadorService`.

### `ExecutarMenu()` — menus sem repetição
O `Program.cs` usa um único método que gera qualquer menu a partir de um dicionário de opções, eliminando a repetição do padrão loop/switch nos três menus secundários.

### `Executar()` — tratamento de erros centralizado
Envolve qualquer operação num try/catch, eliminando blocos repetidos em todos os métodos de escrita.

---

## 🗃️ Base de Dados SQLite

O ficheiro `movies.db` é criado automaticamente na pasta de execução na primeira vez que a aplicação corre.

### Esquema das tabelas

```sql
CREATE TABLE Categorias (
    Id   INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL
);

CREATE TABLE Realizadores (
    Id   INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    Pais TEXT NOT NULL
);

CREATE TABLE Filmes (
    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo        TEXT    NOT NULL,
    Ano           INTEGER NOT NULL,
    Lingua        TEXT    NOT NULL,
    Classificacao REAL    NOT NULL,
    CategoriaId   INTEGER NOT NULL REFERENCES Categorias(Id),
    RealizadorId  INTEGER NOT NULL REFERENCES Realizadores(Id)
);
```

---

## 🚀 Como Executar

1. Clonar o repositório
2. Abrir `Projeto_gestaoFilmes.sln` no Visual Studio 2022
3. Garantir que o pacote NuGet está instalado no projeto `Data`:
   ```
   Install-Package Microsoft.Data.Sqlite
   ```
4. Definir `GestaoFilme.appConsola` como projeto de arranque
5. Executar com **F5** ou **Ctrl+F5**

> Na primeira execução, o ficheiro `movies.db` é criado automaticamente.

---

## 📋 Requisitos

- [.NET 6.0+](https://dotnet.microsoft.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- Pacote NuGet: `Microsoft.Data.Sqlite`

---

## 📌 Commits por Fase

| Commit | Descrição |
|--------|-----------|
| `Conclusão Parte 1` | Entidade Filme + CRUD + persistência em memória |
| `Conclusão Parte 2` | Entidades Categoria e Realizador + CRUD |
| `Conclusão Parte 3` | Relações entre entidades + persistência SQLite + refactoring UI |

---

## 👤 Autor

Projeto desenvolvido no âmbito da formação em C# — Arquitetura em Camadas.
