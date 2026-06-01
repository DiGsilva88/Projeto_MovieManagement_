# 🎬 MovieManagement

Aplicação de gestão de filmes desenvolvida em C# com arquitetura em camadas, interfaces, regras de negócio e persistência de dados.

---

## 📁 Estrutura do Projeto

```
Projeto_gestaoFilmes.sln
├── Domain                          # Entidades e Interfaces
│   ├── Entidades/
│   │   └── Filme.cs
│   └── Interfaces/
│       └── IFilmeRepository.cs
│
├── Data                            # Persistência de Dados
│   └── Repositorio/
│       └── FilmeRepositoryMemory.cs
│
├── Business                        # Regras de Negócio
│   └── Serviços/
│       └── FilmeService.cs
│
└── GestaoFilme.appConsola          # Interface com o Utilizador (UI)
    └── Program.cs
```

---

## 🏗️ Arquitetura

O projeto segue uma **arquitetura em camadas**, onde cada camada tem uma responsabilidade bem definida:

| Camada | Projeto | Responsabilidade |
|--------|---------|-----------------|
| **UI** | `GestaoFilme.appConsola` | Interação com o utilizador (menus, inputs) |
| **Business** | `Business` | Regras de negócio e validações |
| **Data** | `Data` | Persistência dos dados |
| **Domain** | `Domain` | Entidades e contratos (interfaces) |

### Dependências entre camadas

```
UI  →  Business  →  Domain
              Data  →  Domain
UI  →  Data (apenas para instanciar o repositório)
```

> **Importante:** A Business nunca referencia a Data diretamente. O repositório é injetado via construtor (Injeção de Dependência).

---

## ✨ Funcionalidades

### Parte 1 — Filmes
- ✅ Adicionar filme
- ✅ Listar filmes
- ✅ Procurar filme por título
- ✅ Remover filme

### Parte 2 — Categorias e Realizadores *(a implementar)*
- Adicionar / Listar / Procurar / Remover categorias
- Adicionar / Listar / Procurar / Remover realizadores

### Parte 3 — Relações + SQLite *(a implementar)*
- Associar categoria e realizador a cada filme
- Persistência em SQLite (mantendo compatibilidade com persistência em memória)

---

## 🗂️ Entidade Filme

```csharp
public class Filme
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public int Ano { get; set; }
    public string Lingua { get; set; }
    public double Classificacao { get; set; }
}
```

---

## ✅ Regras de Negócio

### Filme
- O **título é obrigatório**
- Não podem existir **títulos duplicados**
- A **classificação** deve estar entre **0 e 5**

---

## 🔌 Persistência

A arquitetura permite trocar a persistência **sem alterar UI, Business ou Domain**.

| Fase | Tipo | Classe |
|------|------|--------|
| Parte 1 e 2 | Memória | `FilmeRepositoryMemory` |
| Parte 3 | SQLite | `FilmeRepositorySQLite` *(a implementar)* |

Para trocar, basta alterar a instanciação no `Program.cs`:

```csharp
// Memória
IFilmeRepository repo = new FilmeRepositoryMemory();

// SQLite (Parte 3)
// IFilmeRepository repo = new FilmeRepositorySQLite();

FilmeService servico = new FilmeService(repo);
```

---

## 🚀 Como Executar

1. Clonar o repositório
2. Abrir a solução `Projeto_gestaoFilmes.sln` no Visual Studio
3. Definir `GestaoFilme.appConsola` como projeto de arranque
4. Executar com **F5** ou **Ctrl + F5**

---

## 📋 Requisitos

- [.NET 6.0+](https://dotnet.microsoft.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- *(Parte 3)* Pacote NuGet: `Microsoft.Data.Sqlite`

---

## 📌 Commits por Fase

| Commit | Descrição |
|--------|-----------|
| `Conclusão Parte 1` | Entidade Filme + CRUD + persistência em memória |
| `Conclusão Parte 2` | Entidades Categoria e Realizador |
| `Conclusão Parte 3` | Relações entre entidades + persistência SQLite |

---

## 👤 Autor

Projeto desenvolvido no âmbito da formação em C# — Arquitetura em Camadas.
