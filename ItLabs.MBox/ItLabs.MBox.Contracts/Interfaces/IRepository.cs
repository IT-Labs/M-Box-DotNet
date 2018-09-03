using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IRepository <T> where T : IEntity
    {
        IQueryable<T> GetAll();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
