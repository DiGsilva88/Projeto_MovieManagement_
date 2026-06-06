namespace Domain.Interfaces
{
    /// <summary>
    /// Contrato para repositório de Categorias.
    /// Renomeado de ICategoryRepository para ICategoriaRepository
    /// para manter consistência com o resto do projeto (tudo em português).
    /// </summary>
    public interface ICategoriaRepository
    {
        void Add(Domain.Entidades.Categoria categoria);
        List<Domain.Entidades.Categoria> GetAll();
        Domain.Entidades.Categoria? GetById(int id);
        Domain.Entidades.Categoria? GetByNome(string nome);  // necessário para verificar duplicados
        void Remove(int id);
    }
}
