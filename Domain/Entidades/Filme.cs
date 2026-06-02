using System;

namespace Domain.Entidades
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Lingua { get; set; } = string.Empty;
        public double Classificacao { get; set; }

        public int RealizadorId { get; set; }
        public int CategoriaId { get; set; }

        public Filme()
        {
        }

        public Filme(
            int id,
            string titulo,
            int ano,
            string lingua,
            double classificacao,
            int categoriaId,
            int realizadorId)
        {


            Id = id;
            Titulo = titulo;
            Ano = ano;
            Lingua = lingua;
            Classificacao = classificacao;

            CategoriaId = categoriaId;
            RealizadorId = realizadorId;
        }

        public override string ToString()
        {
            return $"[{Id}] {Titulo} ({Ano}) | Língua: {Lingua} | Classificação: {Classificacao}/5 | CatID: {CategoriaId} | RealizadorID: {RealizadorId}";
        }
    }
}









