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
            Console.WriteLine("║    GESTÃO DE FILMES      ║");
            Console.WriteLine("╠══════════════════════════╣");
            Console.WriteLine("║  1 - Adicionar filme     ║");
            Console.WriteLine("║  2 - Listar filmes       ║");
            Console.WriteLine("║  3 - Procurar filme      ║");
            Console.WriteLine("║  4 - Remover filme       ║");
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
            string titulo = Console.ReadLine();

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
