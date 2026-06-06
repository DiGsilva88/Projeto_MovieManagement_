using Domain.Entidades;
using Domain.Interfaces;

namespace Business.Servicos
{
    /// <summary>
    /// Serviço de Categorias — regras de negócio para categorias.
    /// </summary>
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repositorio;

        public CategoriaService(ICategoriaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public void AdicionarCategoria(Categoria categoria)
        {
            // Regra: nome obrigatório
            if (string.IsNullOrWhiteSpace(categoria.Nome))
                throw new ArgumentException("O nome da categoria é obrigatório.");

            // Regra: sem categorias duplicadas
            if (_repositorio.GetByNome(categoria.Nome) != null)
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{categoria.Nome}'.");

            _repositorio.Add(categoria);
        }

        public List<Categoria> ListarCategorias() => _repositorio.GetAll();

        public Categoria? ProcurarCategoria(int id) => _repositorio.GetById(id);

        public void RemoverCategoria(int id)
        {
            if (_repositorio.GetById(id) == null)
                throw new InvalidOperationException($"Categoria com ID {id} não encontrada.");

            _repositorio.Remove(id);
        }
    }
}
