using Domain.Entidades;
using Domain.Interfaces;

namespace Business.Servicos
{
    /// <summary>
    /// Serviço de Filmes — contém todas as regras de negócio.
    /// Parte 3: recebe também categoriaRepo e realizadorRepo para validar relações.
    /// </summary>
    public class FilmeService
    {
        private readonly IFilmeRepository _repositorio;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IRealizadorRepository _realizadorRepo;

        public FilmeService(
            IFilmeRepository repositorio,
            ICategoriaRepository categoriaRepo,
            IRealizadorRepository realizadorRepo)
        {
            _repositorio = repositorio;
            _categoriaRepo = categoriaRepo;
            _realizadorRepo = realizadorRepo;
        }

        public void AdicionarFilme(Filme filme)
        {
            // Regra: título obrigatório
            if (string.IsNullOrWhiteSpace(filme.Titulo))
                throw new ArgumentException("O título é obrigatório.");

            // Regra: classificação entre 0 e 5
            if (filme.Classificacao < 0 || filme.Classificacao > 5)
                throw new ArgumentException("A classificação deve estar entre 0 e 5.");

            // Regra: sem títulos duplicados
            if (_repositorio.GetByTitulo(filme.Titulo) != null)
                throw new InvalidOperationException($"Já existe um filme com o título '{filme.Titulo}'.");

            // Regra Parte 3: a categoria deve existir
            if (_categoriaRepo.GetById(filme.CategoriaId) == null)
                throw new InvalidOperationException($"A categoria com ID {filme.CategoriaId} não existe. Adicione a categoria primeiro.");

            // Regra Parte 3: o realizador deve existir
            if (_realizadorRepo.GetById(filme.RealizadorId) == null)
                throw new InvalidOperationException($"O realizador com ID {filme.RealizadorId} não existe. Adicione o realizador primeiro.");

            _repositorio.Add(filme);
        }

        public List<Filme> ListarFilmes() => _repositorio.GetAll();

        public Filme? ProcurarFilme(string titulo) => _repositorio.GetByTitulo(titulo);

        public void RemoverFilme(int id)
        {
            if (_repositorio.GetById(id) == null)
                throw new InvalidOperationException($"Filme com ID {id} não encontrado.");

            _repositorio.Remove(id);
        }
    }
}
