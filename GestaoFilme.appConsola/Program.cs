using Business.Servicos;
using Domain.Entidades;
using Domain.Interfaces;
using Data.Repositorio;
using Data.Repositorio.SQLite;  // Parte 3

namespace GestaoFilme.appConsola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ─── Configuração da persistência ─────────────────────────────
            // Parte 3: usar SQLite. Para voltar à memória, comente as linhas
            // SQLite e descomente as linhas Memory abaixo.

            const string connectionString = "Data Source=movies.db";
            DatabaseInitializer.Initialize(connectionString);  // cria tabelas se não existirem

            // --- SQLite (Parte 3 - ativo) ---
            IFilmeRepository      filmeRepo     = new FilmeRepositorySQLite(connectionString);
            ICategoriaRepository  categoriaRepo = new CategoriaRepositorySQLite(connectionString);
            IRealizadorRepository realizadorRepo = new RealizadorRepositorySQLite(connectionString);

            // --- Memória (Partes 1 e 2 - comentado) ---
            // IFilmeRepository      filmeRepo     = new FilmeRepositoryMemory();
            // ICategoriaRepository  categoriaRepo = new CategoriaRepositoryMemory();
            // IRealizadorRepository realizadorRepo = new RealizadorRepositoryMemory();

            // Serviços — Business layer
            // FilmeService recebe os 3 repositórios para poder validar relações (Parte 3)
            FilmeService      filmeService     = new FilmeService(filmeRepo, categoriaRepo, realizadorRepo);
            CategoriaService  categoriaService = new CategoriaService(categoriaRepo);
            RealizadorService realizadorService = new RealizadorService(realizadorRepo);

            // ─── Loop principal do menu ────────────────────────────────────
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcao = Console.ReadLine() ?? "";

                switch (opcao)
                {
                    case "1":  AdicionarFilme(filmeService, categoriaService, realizadorService); break;
                    case "2":  ListarFilmes(filmeService, categoriaService, realizadorService); break;
                    case "3":  ProcurarFilme(filmeService, categoriaService, realizadorService); break;
                    case "4":  RemoverFilme(filmeService); break;
                    case "5":  AdicionarCategoria(categoriaService); break;
                    case "6":  ListarCategorias(categoriaService); break;
                    case "7":  ProcurarCategoria(categoriaService); break;
                    case "8":  RemoverCategoria(categoriaService); break;
                    case "9":  AdicionarRealizador(realizadorService); break;
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

        // ─── MENU ──────────────────────────────────────────────────────────

        static void MostrarMenu()
        {
            Console.WriteLine("\n╔══════════════════════════════╗");
            Console.WriteLine("║       GESTÃO DE FILMES       ║");
            Console.WriteLine("╠══════════════════════════════╣");
            Console.WriteLine("║  --- Filmes ---               ║");
            Console.WriteLine("║  1  - Adicionar filme         ║");
            Console.WriteLine("║  2  - Listar filmes           ║");
            Console.WriteLine("║  3  - Procurar filme          ║");
            Console.WriteLine("║  4  - Remover filme           ║");
            Console.WriteLine("║  --- Categorias ---           ║");
            Console.WriteLine("║  5  - Adicionar categoria     ║");
            Console.WriteLine("║  6  - Listar categorias       ║");
            Console.WriteLine("║  7  - Procurar categoria      ║");
            Console.WriteLine("║  8  - Remover categoria       ║");
            Console.WriteLine("║  --- Realizadores ---         ║");
            Console.WriteLine("║  9  - Adicionar realizador    ║");
            Console.WriteLine("║  10 - Listar realizadores     ║");
            Console.WriteLine("║  11 - Procurar realizador     ║");
            Console.WriteLine("║  12 - Remover realizador      ║");
            Console.WriteLine("║  0  - Sair                    ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.Write("Escolha uma opção: ");
        }

        // ─── FILMES ────────────────────────────────────────────────────────

        static void AdicionarFilme(FilmeService filmeService, CategoriaService catService, RealizadorService realService)
        {
            Console.WriteLine("\n--- ADICIONAR FILME ---");

            // Mostra categorias e realizadores disponíveis para o utilizador saber os IDs
            ListarCategorias(catService);
            ListarRealizadores(realService);

            Console.Write("\nTítulo: ");
            string titulo = Console.ReadLine() ?? "";

            int ano = LerInteiro("Ano: ");

            Console.Write("Língua: ");
            string lingua = Console.ReadLine() ?? "";

            double classificacao = LerDouble("Classificação (0 a 5): ", 0, 5);

            int categoriaId = LerInteiro("ID da Categoria: ");
            int realizadorId = LerInteiro("ID do Realizador: ");

            Filme novoFilme = new Filme(0, titulo, ano, lingua, classificacao, categoriaId, realizadorId);

            try
            {
                filmeService.AdicionarFilme(novoFilme);
                Console.WriteLine("✔ Filme adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        static void ListarFilmes(FilmeService filmeService, CategoriaService catService, RealizadorService realService)
        {
            Console.WriteLine("\n--- LISTA DE FILMES ---");
            var lista = filmeService.ListarFilmes();

            if (lista.Count == 0)
            {
                Console.WriteLine("Não existem filmes registados.");
                return;
            }

            foreach (var f in lista)
            {
                // Mostra o nome da categoria e do realizador em vez do ID
                string nomeCategoria  = catService.ProcurarCategoria(f.CategoriaId)?.Nome ?? "—";
                string nomeRealizador = realService.ProcurarRealizador(f.RealizadorId)?.Nome ?? "—";

                Console.WriteLine($"[{f.Id}] {f.Titulo} | {f.Ano} | {f.Lingua} | ⭐ {f.Classificacao}/5 | {nomeCategoria} | {nomeRealizador}");
            }
        }

        static void ProcurarFilme(FilmeService filmeService, CategoriaService catService, RealizadorService realService)
        {
            Console.WriteLine("\n--- PROCURAR FILME ---");
            Console.Write("Título a procurar: ");
            string titulo = Console.ReadLine() ?? "";

            var filme = filmeService.ProcurarFilme(titulo);

            if (filme == null)
            {
                Console.WriteLine("Filme não encontrado.");
            }
            else
            {
                string nomeCategoria  = catService.ProcurarCategoria(filme.CategoriaId)?.Nome ?? "—";
                string nomeRealizador = realService.ProcurarRealizador(filme.RealizadorId)?.Nome ?? "—";
                Console.WriteLine($"Encontrado: [{filme.Id}] {filme.Titulo} | {filme.Ano} | {filme.Lingua} | ⭐ {filme.Classificacao}/5 | {nomeCategoria} | {nomeRealizador}");
            }
        }

        static void RemoverFilme(FilmeService servico)
        {
            Console.WriteLine("\n--- REMOVER FILME ---");
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

        // ─── CATEGORIAS ────────────────────────────────────────────────────

        static void AdicionarCategoria(CategoriaService servico)
        {
            Console.WriteLine("\n--- ADICIONAR CATEGORIA ---");
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? "";

            try
            {
                servico.AdicionarCategoria(new Categoria(0, nome));
                Console.WriteLine("✔ Categoria adicionada com sucesso!");
            }
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); }
        }

        static void ListarCategorias(CategoriaService servico)
        {
            Console.WriteLine("\n--- LISTA DE CATEGORIAS ---");
            var lista = servico.ListarCategorias();

            if (lista.Count == 0) { Console.WriteLine("Não existem categorias registadas."); return; }
            foreach (var c in lista)
                Console.WriteLine($"[{c.Id}] {c.Nome}");
        }

        static void ProcurarCategoria(CategoriaService servico)
        {
            Console.WriteLine("\n--- PROCURAR CATEGORIA ---");
            int id = LerInteiro("ID da categoria: ");
            var categoria = servico.ProcurarCategoria(id);

            if (categoria == null) Console.WriteLine("Categoria não encontrada.");
            else Console.WriteLine($"[{categoria.Id}] {categoria.Nome}");
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
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); }
        }

        // ─── REALIZADORES ──────────────────────────────────────────────────

        static void AdicionarRealizador(RealizadorService servico)
        {
            Console.WriteLine("\n--- ADICIONAR REALIZADOR ---");
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? "";
            Console.Write("Nacionalidade: ");
            string nacionalidade = Console.ReadLine() ?? "";

            try
            {
                servico.AdicionarRealizador(new Realizador(0, nome, nacionalidade));
                Console.WriteLine("✔ Realizador adicionado com sucesso!");
            }
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); }
        }

        static void ListarRealizadores(RealizadorService servico)
        {
            Console.WriteLine("\n--- LISTA DE REALIZADORES ---");
            var lista = servico.ListarRealizadores();

            if (lista.Count == 0) { Console.WriteLine("Não existem realizadores registados."); return; }
            foreach (var r in lista)
                Console.WriteLine($"[{r.Id}] {r.Nome} ({r.Nacionalidade})");
        }

        static void ProcurarRealizador(RealizadorService servico)
        {
            Console.WriteLine("\n--- PROCURAR REALIZADOR ---");
            int id = LerInteiro("ID do realizador: ");
            var realizador = servico.ProcurarRealizador(id);

            if (realizador == null) Console.WriteLine("Realizador não encontrado.");
            else Console.WriteLine($"[{realizador.Id}] {realizador.Nome} ({realizador.Nacionalidade})");
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
            catch (Exception ex) { Console.WriteLine("Erro: " + ex.Message); }
        }

        // ─── UTILITÁRIOS ───────────────────────────────────────────────────

        /// <summary>Lê e valida um número inteiro positivo.</summary>
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

        /// <summary>Lê e valida um número decimal dentro de um intervalo.</summary>
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
