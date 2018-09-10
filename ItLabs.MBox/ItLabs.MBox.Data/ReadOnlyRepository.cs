using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ItLabs.MBox.Data
{
    public class ReadOnlyRepository<TContext> : IReadOnlyRepository where TContext : MBoxDbContext
    {
        protected readonly TContext context;
        public ReadOnlyRepository(TContext contextInjected)
        {
            context = contextInjected;
        }
        protected virtual IQueryable<Entity> GetQueryable<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        where Entity : class, IEntity
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<Entity> query = context.Set<Entity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public virtual IEnumerable<Entity> GetAll<Entity>(
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        where Entity : class, IEntity
        {
            return GetQueryable<Entity>(null, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual IEnumerable<Entity> Get<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        where Entity : class, IEntity
        {
            return GetQueryable<Entity>(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual Entity GetOne<Entity>(
        Expression<Func<Entity, bool>> filter = null,
        string includeProperties = "")
        where Entity : class, IEntity
        {
            return GetQueryable<Entity>(filter, null, includeProperties).SingleOrDefault();
        }

        public virtual Entity GetFirst<Entity>(
       Expression<Func<Entity, bool>> filter = null,
       Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
       string includeProperties = "")
       where Entity : class, IEntity
        {
            return GetQueryable<Entity>(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public virtual Entity GetById<Entity>(object id)
        where Entity : class, IEntity
        {
            return context.Set<Entity>().Find(id);
        }
        public virtual int GetCount<Entity>(Expression<Func<Entity, bool>> filter = null)
        where Entity : class, IEntity
        {
            return GetQueryable<Entity>(filter).Count();
        }
        public virtual bool GetExists<Entity>(Expression<Func<Entity, bool>> filter = null)
        where Entity : class, IEntity
        {
            return GetQueryable<Entity>(filter).Any();
        }

    }
}
