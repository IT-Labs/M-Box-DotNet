using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {

        private readonly MBoxDbContext _mboxDbContext;
        private DbSet<T> _entities;

        public Repository(MBoxDbContext context)
        {
            _mboxDbContext = context;
            _entities = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable();
        }
        public T Get(int id)
        {
            return _entities.SingleOrDefault(y => y.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _mboxDbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _mboxDbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _mboxDbContext.SaveChanges();
        }

    }
}
