using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        Category? GetById(int id);
        List<Category> GetAll();
        void Delete(int id);

    }


}
