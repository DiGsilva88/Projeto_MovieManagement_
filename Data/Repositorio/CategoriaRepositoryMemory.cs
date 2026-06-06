using Domain.Entidades;
using Domain.Interfaces;

namespace Data.Repositorio
{
    /// <summary>
    /// Persistência em memória para Categorias.
    /// Implementa ICategoriaRepository (era ICategoryRepository — corrigido para consistência).
    /// </summary>
    public class CategoriaRepositoryMemory : ICategoriaRepository
    {
        private readonly List<Categoria> _categorias = new();
        private int _nextId = 1;

        public void Add(Categoria categoria)
        {
            categoria.Id = _nextId++;
            _categorias.Add(categoria);
        }

        public List<Categoria> GetAll() => _categorias;

        public Categoria? GetById(int id) =>
            _categorias.FirstOrDefault(c => c.Id == id);

        // Necessário para verificar duplicados no CategoriaService
        public Categoria? GetByNome(string nome) =>
            _categorias.FirstOrDefault(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        public void Delete(int id) =>
            _categorias.RemoveAll(c => c.Id == id);

        // Alias para manter compatibilidade com a interface
        public void Remove(int id) => Delete(id);
    }
}
