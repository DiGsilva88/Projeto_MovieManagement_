using Domain.Entidades;
using Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Business.Servicos
{
    public class FilmeService
    {
        private readonly IFilmeRepository _repository;

        // Construtor com Injeção de Dependência
        public FilmeService(IFilmeRepository repository)
        {
            // Validação direta: garante que o repositório não é nulo


            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;
        }

        public void AdicionarFilme(Filme filme)
        {
            if (string.IsNullOrWhiteSpace(filme.Titulo))
            {
                throw new Exception("Título é obrigatório");
            }

            if (_repository.GetByTitulo(filme.Titulo) != null)
            {
                throw new Exception("Já existe um filme com esse título");
            }

            if (filme.Classificacao < 0 || filme.Classificacao > 5)
            {
                throw new Exception("Classificação deve estar entre 0 e 5");
            }

            _repository.Add(filme);
        }

        public List<Filme> ListarFilmes()
        {
            return _repository.GetAll();
        }

        public Filme ProcurarFilme(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título de busca não pode ser vazio.");
            }

            return _repository.GetByTitulo(titulo);
        }

        public void RemoverFilme(int id)
        {
            _repository.Remove(id);
        }
    }
}
