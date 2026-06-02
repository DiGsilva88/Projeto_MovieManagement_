using Domain.Entidades;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        void Add(Categoria categoria);

        Categoria? GetById(int id);

        List<Categoria> GetAll();

        void Delete(int id);
    }
}