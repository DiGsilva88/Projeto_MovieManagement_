using Business.Servicos;
using Domain.Entidades;
using Domain.Interfaces;
using Data.Repositorio;

namespace GestaoFilme.appConsola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ─── Configuração inicial ───────────────────────────────────────
            // Para trocar para SQLite basta substituir FilmeRepositoryMemory
            // por FilmeRepositorySQLite — sem tocar em mais nada!

            IFilmeRepository repositorio = new FilmeRepositoryMemory();
            FilmeService servico = new FilmeService(repositorio);
            ICategoryRepository categoriaRepo = new CategoriaRepositoryMemory();
            CategoriaService categoriaService = new CategoriaService(categoriaRepo);

            IRealizadorRepository realizadorRepo = new RealizadorRepositoryMemory();
            RealizadorService realizadorService = new RealizadorService(realizadorRepo);

            // ─── Loop principal do menu ─────────────────────────────────────
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcao = Console.ReadLine() ?? "";

                switch (opcao)
                {
                    case "1": AdicionarFilme(servico); break;
                    case "2": ListarFilmes(servico); break;
                    case "3": ProcurarFilme(servico); break;
                    case "4": RemoverFilme(servico); break;
                    case "5": AdicionarCategoria(categoriaService); break;
                    case "6": ListarCategorias(categoriaService); break;
                    case "7": ProcurarCategoria(categoriaService); break;
                    case "8": RemoverCategoria(categoriaService); break;

                    case "9": AdicionarRealizador(realizadorService); break;
                    case "10": ListarRealizadores(realizadorService); break;
                    case "11": ProcurarRealizador(realizadorService); break;
                    case "12": RemoverRealizador(realizadorService); break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\nAplicação terminada. Até logo!");
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        break;
                }
            }
        }

        // ───────────────────────────────────────────────────
        // MENU
        // ───────────────────────────────────────────────────

        
        /// Mostra o menu principal no ecrã.
        
        static void MostrarMenu()
        {
            Console.WriteLine("\n╔══════════════════════════╗");
            Console.WriteLine("║     GESTÃO DE FILMES     ║");
            Console.WriteLine("╠══════════════════════════╣");
            Console.WriteLine("║  1 - Adicionar filme     ║");
            Console.WriteLine("║  2 - Listar filmes       ║");
            Console.WriteLine("║  3 - Procurar filme      ║");
            Console.WriteLine("║  4 - Remover filme       ║");
            Console.WriteLine("║  5 - Adicionar categoria ║");
            Console.WriteLine("║  6 - Listar categorias   ║");
            Console.WriteLine("║  7 - Procurar categorias ║");
            Console.WriteLine("║  8 - Remover categoria   ║");
            Console.WriteLine("║  5 -Adicionar realizador ║");
            Console.WriteLine("║  6 - Listar   realizador ║");
            Console.WriteLine("║  7 - Procurar realizador ║");
            Console.WriteLine("║  8 - Remover realizador  ║");
            Console.WriteLine("║  0 - Sair                ║");
            Console.WriteLine("╚══════════════════════════╝");
            Console.Write("Escolha uma opção: ");
        }

        // ───────────────────────────────────────────────────
        // OPÇÕES DO MENU
        // ───────────────────────────────────────────────────


        /// Recolhe os dados do utilizador e adiciona um novo filme.
        /// As validações de negócio são feitas no FilmeService.

        static void AdicionarFilme(FilmeService servico)
        {
            Console.WriteLine("\n--- ADICIONAR FILME ---");

            Console.Write("Título: ");
            string titulo = Console.ReadLine() ?? "";

            int ano = LerInteiro("Ano: ");

            Console.Write("Língua: ");
            string lingua = Console.ReadLine() ?? "";

            double classificacao = LerDouble("Classificação (0 a 5): ", 0, 5);

            int categoriaId = LerInteiro("ID da Categoria: ");

            int realizadorId = LerInteiro("ID do Realizador: ");

            Filme novoFilme = new Filme(
                0,
                titulo,
                ano,
                lingua,
                classificacao,
                categoriaId,
                realizadorId
            );

            try
            {
                servico.AdicionarFilme(novoFilme);
                Console.WriteLine("✔ Filme adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        /// Lista todos os filmes registados.

        static void ListarFilmes(FilmeService servico)
        {
            Console.WriteLine("\n--- LISTA DE FILMES ---");

            var lista = servico.ListarFilmes();

            // Verifica se existem filmes antes de listar
            if (lista.Count == 0)
            {
                Console.WriteLine("Não existem filmes registados.");
                return;
            }

            foreach (var f in lista)
            {
                Console.WriteLine($"[{f.Id}] {f.Titulo} | {f.Ano} | {f.Lingua} | ⭐ {f.Classificacao}/5");
            }
        }

        
        /// Procura um filme pelo título e mostra o resultado.
        
        static void ProcurarFilme(FilmeService servico)
        {
            Console.WriteLine("\n--- PROCURAR FILME ---");

            Console.Write("Título a procurar: ");
            string titulo = Console.ReadLine() ?? "";

            var filme = servico.ProcurarFilme(titulo);

            if (filme == null)
            {
                Console.WriteLine("Filme não encontrado.");
            }
            else
            {
                Console.WriteLine($"Encontrado: [{filme.Id}] {filme.Titulo} | {filme.Ano} | {filme.Lingua} | ⭐ {filme.Classificacao}/5");
            }
        }

        
        /// Remove um filme pelo ID.
        
        static void RemoverFilme(FilmeService servico)
        {
            Console.WriteLine("\n--- REMOVER FILME ---");

            // Mostra a lista antes de pedir o ID — mais intuitivo para o utilizador
            ListarFilmes(servico);

            int id = LerInteiro("ID do filme a remover: ");

            try
            {
                servico.RemoverFilme(id);
                Console.WriteLine("✔ Filme removido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // MÉTODOS AUXILIARES — reutilizáveis em qualquer opção do menu
        // ────────────────────────────────────────────────────────────────────

        
        /// Lê e valida um número inteiro positivo.
        /// Repete a pergunta até o utilizador introduzir um valor válido.
        
        static int LerInteiro(string mensagem)
        {
            int valor;
            while (true)
            {
                Console.Write(mensagem);
                if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
                    return valor;

                Console.WriteLine("Valor inválido. Introduza um número inteiro positivo.");
            }
        }
        ///metodos da categoria
        static void AdicionarCategoria(CategoriaService servico)
        {
    Console.WriteLine("\n--- ADICIONAR CATEGORIA ---");

    Console.Write("Nome: ");
    string nome = Console.ReadLine() ?? "";

        Categoria categoria = new Categoria(0, nome);

    try
    {
        servico.AdicionarCategoria(categoria);
        Console.WriteLine("✔ Categoria adicionada com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
    }
}

static void ListarCategorias(CategoriaService servico)
{
    Console.WriteLine("\n--- LISTA DE CATEGORIAS ---");

    var lista = servico.ListarCategorias();

    if (lista.Count == 0)
    {
        Console.WriteLine("Não existem categorias registadas.");
        return;
    }

    foreach (var categoria in lista)
    {
        Console.WriteLine($"[{categoria.Id}] {categoria.Nome}");
    }
}

static void ProcurarCategoria(CategoriaService servico)
{
    Console.WriteLine("\n--- PROCURAR CATEGORIA ---");

    int id = LerInteiro("ID da categoria: ");

    var categoria = servico.ProcurarCategoria(id);

    if (categoria == null)
        Console.WriteLine("Categoria não encontrada.");
    else
        Console.WriteLine($"[{categoria.Id}] {categoria.Nome}");
}

static void RemoverCategoria(CategoriaService servico)
{
    Console.WriteLine("\n--- REMOVER CATEGORIA ---");

    int id = LerInteiro("ID da categoria: ");

    try
    {
        servico.RemoverCategoria(id);
        Console.WriteLine("✔ Categoria removida com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
    }
}

static void AdicionarRealizador(RealizadorService servico)
{
    Console.WriteLine("\n--- ADICIONAR REALIZADOR ---");

    Console.Write("Nome: ");
    string nome = Console.ReadLine() ?? "";

    Console.Write("Nacionalidade: ");
    string nacionalidade = Console.ReadLine() ?? "";

    Realizador realizador = new Realizador(
        0,
        nome,
        nacionalidade
    );

    try
    {
        servico.AdicionarRealizador(realizador);
        Console.WriteLine("✔ Realizador adicionado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
    }
}

static void ListarRealizadores(RealizadorService servico)
{
    Console.WriteLine("\n--- LISTA DE REALIZADORES ---");

    var lista = servico.ListarRealizadores();

    if (lista.Count == 0)
    {
        Console.WriteLine("Não existem realizadores registados.");
        return;
    }

    foreach (var r in lista)
    {
        Console.WriteLine($"[{r.Id}] {r.Nome} ({r.Nacionalidade})");
    }
}

static void ProcurarRealizador(RealizadorService servico)
{
    Console.WriteLine("\n--- PROCURAR REALIZADOR ---");

    int id = LerInteiro("ID do realizador: ");

    var realizador = servico.ProcurarRealizador(id);

    if (realizador == null)
        Console.WriteLine("Realizador não encontrado.");
    else
        Console.WriteLine($"[{realizador.Id}] {realizador.Nome} ({realizador.Nacionalidade})");
}

static void RemoverRealizador(RealizadorService servico)
{
    Console.WriteLine("\n--- REMOVER REALIZADOR ---");

    int id = LerInteiro("ID do realizador: ");

    try
    {
        servico.RemoverRealizador(id);
        Console.WriteLine("✔ Realizador removido com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro: " + ex.Message);
    }
}

/// Lê e valida um número decimal dentro de um intervalo definido.
/// Repete a pergunta até o utilizador introduzir um valor válido.

static double LerDouble(string mensagem, double min, double max)
        {
            double valor;
            while (true)
            {
                Console.Write(mensagem);
                if (double.TryParse(Console.ReadLine(), out valor) && valor >= min && valor <= max)
                    return valor;

                Console.WriteLine($"Valor inválido. Deve estar entre {min} e {max}.");
            }
        }

    }
}
