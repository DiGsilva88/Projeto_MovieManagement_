namespace Domain.Entidades
{
    /// <summary>
    /// Entidade Filme — atualizada na Parte 3 com CategoriaId e RealizadorId.
    /// </summary>
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public string Lingua { get; set; }
        public double Classificacao { get; set; }

        // Parte 3 — relação com Categoria e Realizador
        public int CategoriaId { get; set; }
        public int RealizadorId { get; set; }

        public Filme(int id, string titulo, int ano, string lingua, double classificacao, int categoriaId, int realizadorId)
        {
            Id = id;
            Titulo = titulo;
            Ano = ano;
            Lingua = lingua;
            Classificacao = classificacao;
            CategoriaId = categoriaId;
            RealizadorId = realizadorId;
        }
    }
}
