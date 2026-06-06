using Domain.Entidades;
using Domain.Interfaces;

namespace Business.Servicos
{
    /// <summary>
    /// Serviço de Realizadores — regras de negócio para realizadores.
    /// </summary>
    public class RealizadorService
    {
        private readonly IRealizadorRepository _repositorio;

        public RealizadorService(IRealizadorRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public void AdicionarRealizador(Realizador realizador)
        {
            // Regra: nome obrigatório
            if (string.IsNullOrWhiteSpace(realizador.Nome))
                throw new ArgumentException("O nome do realizador é obrigatório.");

            // Regra: nacionalidade obrigatória
            if (string.IsNullOrWhiteSpace(realizador.Nacionalidade))
                throw new ArgumentException("A nacionalidade do realizador é obrigatória.");

            // Regra: sem realizadores duplicados
            if (_repositorio.GetByNome(realizador.Nome) != null)
                throw new InvalidOperationException($"Já existe um realizador com o nome '{realizador.Nome}'.");

            _repositorio.Add(realizador);
        }

        public List<Realizador> ListarRealizadores() => _repositorio.GetAll();

        public Realizador? ProcurarRealizador(int id) => _repositorio.GetById(id);

        public void RemoverRealizador(int id)
        {
            if (_repositorio.GetById(id) == null)
                throw new InvalidOperationException($"Realizador com ID {id} não encontrado.");

            _repositorio.Remove(id);
        }
    }
}
