using Domain.Entidades;
using Domain.Interfaces;

namespace Data.Repositorio
{
    /// <summary>
    /// Persistência em memória para Realizadores.
    /// Adicionado GetByNome para suportar validação de duplicados.
    /// </summary>
    public class RealizadorRepositoryMemory : IRealizadorRepository
    {
        private readonly List<Realizador> _realizadores = new();
        private int _nextId = 1;

        public void Add(Realizador realizador)
        {
            realizador.Id = _nextId++;
            _realizadores.Add(realizador);
        }

        public List<Realizador> GetAll() => _realizadores;

        public Realizador? GetById(int id) =>
            _realizadores.FirstOrDefault(r => r.Id == id);

        // Necessário para verificar duplicados no RealizadorService
        public Realizador? GetByNome(string nome) =>
            _realizadores.FirstOrDefault(r => r.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        public void Remove(int id) =>
            _realizadores.RemoveAll(r => r.Id == id);
    }
}
