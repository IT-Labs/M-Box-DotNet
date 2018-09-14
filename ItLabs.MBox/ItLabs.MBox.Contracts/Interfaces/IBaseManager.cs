using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IBaseManager<T>
    {
        void Save();

        void Create(T entity, int createdBy);

        void Delete(object id);

        void Delete(T entity);

        void Update(T entity, int? modifiedBy);

        IEnumerable<T> GetAll(
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null);

        IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null);

        T GetOne(
        Expression<Func<T, bool>> filter = null,
        string includeProperties = "");

        T GetById(object id);

    }
}
