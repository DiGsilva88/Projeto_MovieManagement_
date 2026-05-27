using System;


namespace Domain.Entidades
{
    public class Filme
    {
        //Id, Título, Ano, Língua, Classificação


        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public string Lingua { get; set; }
        public double Classificacao { get; set; }



        // Construtor sem parâmetros para permitir criação de instâncias sem fornecer todos os campos
        public Filme()
        {
        }

        public Filme(int id, string titulo, int ano, string lingua, double classificacao)
        {
            Id = id;
            Titulo = titulo;
            Ano = ano;
            Lingua = lingua;
            Classificacao = classificacao;
        }
















    }



}
