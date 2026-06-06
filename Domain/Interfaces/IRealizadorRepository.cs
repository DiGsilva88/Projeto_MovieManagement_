namespace Domain.Interfaces
{
    /// <summary>
    /// Contrato para repositório de Realizadores.
    /// Inclui GetByNome para validação de duplicados no serviço.
    /// </summary>
    public interface IRealizadorRepository
    {
        void Add(Domain.Entidades.Realizador realizador);
        List<Domain.Entidades.Realizador> GetAll();
        Domain.Entidades.Realizador? GetById(int id);
        Domain.Entidades.Realizador? GetByNome(string nome);  // necessário para verificar duplicados
        void Remove(int id);
    }
}
