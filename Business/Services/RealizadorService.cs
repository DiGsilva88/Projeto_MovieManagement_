using Domain.Entidades;
using Domain.Interfaces;

namespace Business.Servicos
{
    public class RealizadorService
    {
        private readonly IRealizadorRepository _repositorio;

        public RealizadorService(IRealizadorRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public void AdicionarRealizador(Realizador realizador)
        {
            if (string.IsNullOrWhiteSpace(realizador.Nome))
                throw new ArgumentException("O nome do realizador é obrigatório.");

            if (string.IsNullOrWhiteSpace(realizador.Nacionalidade))
                throw new ArgumentException("A nacionalidade é obrigatória.");

            _repositorio.Add(realizador);
        }

        public List<Realizador> ListarRealizadores()
        {
            return _repositorio.GetAll();
        }

        public Realizador? ProcurarRealizador(int id)
        {
            return _repositorio.GetById(id);
        }

        public void RemoverRealizador(int id)
        {
            var realizador = _repositorio.GetById(id);

            if (realizador == null)
                throw new ArgumentException("Realizador não encontrado.");

            _repositorio.Remove(id);
        }
    }
}