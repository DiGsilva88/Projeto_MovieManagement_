using Domain.Entidades;
using Domain.Interfaces;
using static System.Net.WebRequestMethods;
using System.Linq;



namespace Data.Repositorio
{
    public class FilmeRepositoryMemory : IFilmeRepository
    {
        private List<Filme> _filmes = new List<Filme>();
        private int _nextId = 1;

       public void Add(Filme filmes)
        {

            filmes.Id = _nextId;
            _nextId++;
            _filmes.Add(filmes);

        }

        List<Filme> IFilmeRepository.GetAll()
        {
            return _filmes;

        }

        Filme IFilmeRepository.GetByTitulo(string titulo)
        {
            return _filmes.FirstOrDefault(f => f.Titulo == titulo);

        }

        void IFilmeRepository.Remove(int id)
        {

            _filmes.RemoveAll(f => f.Id == id);

        }
    }
}
