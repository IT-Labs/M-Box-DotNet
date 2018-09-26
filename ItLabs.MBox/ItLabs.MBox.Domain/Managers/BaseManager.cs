using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class BaseManager<T> : IBaseManager<T> where T : Entity
    {

        private readonly IRepository _repository;
        protected readonly ILogger _logger;
        private IRepository repository;

        public BaseManager(IRepository repository, ILogger<T> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public void Save()
        {
            _repository.Save();
        }

        public void Create(T entity, int createdBy)
        {
            _repository.Create(entity, createdBy);
        }

        public void Delete(object id)
        {
            _repository.Delete<T>(id);
        }

        public void Delete(T entity)
        {
            _repository.Delete<T>(entity);
        }

        public void Update(T entity, int? modifiedBy)
        {
            _repository.Update(entity, modifiedBy);
        }

        public IEnumerable<T> GetAll(
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        {
            return _repository.GetAll(orderBy, includeProperties, skip, take);
        }
        public IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null,
        int? skip = null,
        int? take = null)
        {
            return _repository.Get(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public T GetOne(
        Expression<Func<T, bool>> filter = null,
        string includeProperties = "")
        {
            return _repository.GetOne(filter, includeProperties);
        }


        public T GetById(object id)
        {
            return _repository.GetById<T>(id);
        }
    }
}
