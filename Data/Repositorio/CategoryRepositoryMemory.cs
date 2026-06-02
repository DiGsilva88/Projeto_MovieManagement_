using Domain.Entidades;
using Domain.Interfaces;

namespace Data.Repositorio
{
    public class CategoriaRepositoryMemory : ICategoryRepository
    {
        private readonly List<Categoria> _categorias = new();
        private int _nextId = 1;

        public void Add(Categoria categoria)
        {
            categoria.Id = _nextId++;
            _categorias.Add(categoria);
        }

        public Categoria? GetById(int id)
        {
            return _categorias.FirstOrDefault(c => c.Id == id);
        }

        public List<Categoria> GetAll()
        {
            return _categorias;
        }

        public void Delete(int id)
        {
            _categorias.RemoveAll(c => c.Id == id);
        }
    }
}