using Business.Servicos;
using Domain.Entidades;
using Domain.Interfaces;
using Data.Repositorio;
using System;
using static System.Net.WebRequestMethods;

namespace GestaoFilme.appConsola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Instanciar o repositório fictício (Substitua pela sua implementação real, ex: FilmeSqlServerRepository)
            IFilmeRepository repositorio = new FilmeRepositoryMemory();
            var servicos = new FilmeService(repositorio);

            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\n--- MENU DE FILMES ---");
                Console.WriteLine("1 - Adicionar filme");
                Console.WriteLine("2 - Listar filmes");
                Console.WriteLine("3 - Procurar filme por titulo");
                Console.WriteLine("4 - Remover filme");
                Console.WriteLine("0 - Para sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    Console.Write("Titulo: ");
                    string titulo = Console.ReadLine();

                    // Ler e validar classificação (double entre 0 e 5)
                    double classificacao;
                    while (true)
                    {
                        Console.Write("Classificação (0 a 5): ");
                        string clsInput = Console.ReadLine();
                        if (!double.TryParse(clsInput, out classificacao) || classificacao < 0 || classificacao > 5)
                        {
                            Console.WriteLine("Classificação inválida. Introduza um número entre 0 e 5.");
                            continue;
                        }
                        break;
                    }

                    // Ler e validar ano (int positivo)
                    int ano;
                    while (true)
                    {
                        Console.Write("Ano: ");
                        string anoInput = Console.ReadLine();
                        if (!int.TryParse(anoInput, out ano) || ano <= 0)
                        {
                            Console.WriteLine("Ano inválido. Introduza um número inteiro positivo.");
                            continue;
                        }
                        break;
                    }

                    // Ler língua
                    Console.Write("Língua: ");
                    string lingua = Console.ReadLine();

                    // Criar o objeto Filme para enviar ao serviço
                    Filme novoFilme = new Filme();
                    novoFilme.Titulo = titulo;
                    novoFilme.Classificacao = classificacao;
                    novoFilme.Ano = ano;
                    novoFilme.Lingua = lingua;

                    try
                    {
                        servicos.AdicionarFilme(novoFilme);
                        Console.WriteLine("Filme adicionado com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
                    }
                }
                else if (opcao == "2")
                {
                    var lista = servicos.ListarFilmes();
                    foreach (var f in lista)
                    {
                        Console.WriteLine($"{f.Id} - {f.Titulo} - Classificação: {f.Classificacao}/5");
                    }
                }
                else if (opcao == "3")
                {
                    Console.Write("Título a procurar: ");
                    string tituloBusca = Console.ReadLine();

                    var filme = servicos.ProcurarFilme(tituloBusca);

                    if (filme == null)
                    {
                        Console.WriteLine("Filme não encontrado.");
                    }
                    else
                    {
                        Console.WriteLine($"Encontrado: {filme.Id} - {filme.Titulo} - Classificação: {filme.Classificacao}/5");
                    }
                }
                else if (opcao == "4")
                {
                    Console.Write("ID do filme a remover: ");
                    int id = int.Parse(Console.ReadLine());

                    try
                    {
                        servicos.RemoverFilme(id);
                        Console.WriteLine("Filme removido com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
                    }
                }
                else if (opcao == "0")
                {
                    continuar = false;
                    Console.WriteLine("Aplicação terminada.");
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                }
            }
        }
    }
}


    
