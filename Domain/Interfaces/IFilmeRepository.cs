using Domain.Entidades;

namespace Domain.Interfaces
{
    public interface IFilmeRepository
    {
        void Add(Filme filme);
        List<Filme> GetAll();
        Filme GetByTitulo(string titulo);
        void Remove(int id);


    }
}
