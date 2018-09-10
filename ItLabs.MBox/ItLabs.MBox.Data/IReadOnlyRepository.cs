using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ItLabs.MBox.Data
{
    public interface IReadOnlyRepository
    {
        IEnumerable<Entity> GetAll<Entity>(
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        where Entity : class, IEntity;

        IEnumerable<Entity> Get<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        where Entity : class, IEntity;

        Entity GetOne<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        string includeProperties = null)
        where Entity : class, IEntity;

        Entity GetFirst<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null)
        where Entity : class, IEntity;

        Entity GetById<Entity>(object id)
        where Entity : class, IEntity;

        int GetCount<Entity>(Expression<Func<Entity, bool>> filter = null)
        where Entity : class, IEntity;

        bool GetExists<Entity>(Expression<Func<Entity, bool>> filter = null)
        where Entity : class, IEntity;
    }
}
