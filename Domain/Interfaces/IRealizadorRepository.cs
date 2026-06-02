using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    internal interface IRealizadorRepository
    {

        void Add(Realizador realizador);
        List<Realizador> GetAll();
        Realizador GetById(int id);

        void Remove(int id);
    }
}




