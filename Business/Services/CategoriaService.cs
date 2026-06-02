using Domain.Entidades;
using Domain.Interfaces;

namespace Business.Servicos
{
    public class CategoriaService
    {
        private readonly ICategoryRepository _repositorio;

        public CategoriaService(ICategoryRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public void AdicionarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new ArgumentException("O nome da categoria é obrigatório.");

            var categorias = _repositorio.GetAll();

            if (categorias.Any(c =>
                c.Nome.Equals(categoria.Nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Já existe uma categoria com esse nome.");
            }

            _repositorio.Add(categoria);
        }

        public List<Categoria> ListarCategorias()
        {
            return _repositorio.GetAll();
        }

        public Categoria? ProcurarCategoria(int id)
        {
            return _repositorio.GetById(id);
        }

        public void RemoverCategoria(int id)
        {
            var categoria = _repositorio.GetById(id);

            if (categoria == null)
                throw new ArgumentException("Categoria não encontrada.");

            _repositorio.Delete(id);
        }
    }
}