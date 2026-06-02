using Domain.Entidades;
using Domain.Interfaces;
using System;


namespace Data.Repositorio
{
    public class RealizadorRepositoryMemory : IRealizadorRepository
    {
        private readonly List<Realizador> _realizadores = new();
        private int _nextId = 1;
    

    public void Add(Realizador realizador)
        {
            realizador.Id = _nextId++;
            _realizadores.Add(realizador);
        }

        public List<Realizador> GetAll()
        {
            return _realizadores;
        }

        public Realizador? GetById(int id)
        {
            return _realizadores.FirstOrDefault(r => r.Id == id);
        }

        public void Remove(int id)
        {
            _realizadores.RemoveAll(r => r.Id == id);
        }

    }
}