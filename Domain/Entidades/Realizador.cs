using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entidades
{
    public class Realizador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Nacionalidade { get; set; }

        public Realizador()
        {
        }

        public Realizador(int id, string nome, string nacionalidade)
        {
            Id = id;
            Nome = nome;
            Nacionalidade = nacionalidade;
        }

        public override string ToString()
        {
            return $"[{Id}] {Nome} ({Nacionalidade})";
        }
    }
}
