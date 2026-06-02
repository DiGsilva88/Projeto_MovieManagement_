using Domain.Entidades;

namespace Domain.Interfaces
{
    public interface IFilmeRepository
    {
        void Add(Filme filme);
        List<Filme> GetAll();
        Filme GetByTitulo(string titulo);
        Filme GetById(int id);

        void Remove(int id);


    }
}



