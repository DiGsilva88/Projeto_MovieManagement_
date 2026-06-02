using Domain.Entidades;
using Domain.Interfaces;

namespace Data.Repositorio
{
    public class FilmeRepositoryMemory : IFilmeRepository
    {
        private readonly List<Filme> _filmes = new();
        private int _nextId = 1;

        public void Add(Filme filme)
        {
            filme.Id = _nextId++;
           

            _filmes.Add(filme);
        }

        public List<Filme> GetAll()
        {
            return _filmes;
        }

        public Filme? GetByTitulo(string titulo)
        {
            return _filmes.FirstOrDefault(f => f.Titulo == titulo);
        }

        public Filme? GetById(int id)
        {
            return _filmes.FirstOrDefault(f => f.Id == id);
        }

        public void Remove(int id)
        {
            _filmes.RemoveAll(f => f.Id == id);
        }
    }
}


