using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace ItLabs.MBox.Data
{
    public class Repository<TContext> : ReadOnlyRepository<TContext>, IRepository
    where TContext : MBoxDbContext
    {
        public ILogger _logger { get; set; }
        public Repository(TContext context, ILogger<Repository<TContext>> logger) : base(context) { _logger = logger; }

        public virtual void Create<Entity>(Entity entity, int createdBy)
        where Entity : class, IEntity
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            context.Set<Entity>().Add(entity);
        }

        public virtual void Update<Entity>(Entity entity, int? modifiedBy = null)
            where Entity : class, IEntity
        {
            entity.DateModified = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;
            context.Set<Entity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<Entity>(object id)
            where Entity : class, IEntity
        {
            Entity entity = context.Set<Entity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<Entity>(Entity entity)
            where Entity : class, IEntity
        {
            var dbSet = context.Set<Entity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
      
    }
}
