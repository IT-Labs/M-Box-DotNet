using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Data
{
    public interface IRepository : IReadOnlyRepository
    {
        void Create<Entity>(Entity entity, int createdBy)
        where Entity : class, IEntity;

        void Update<Entity>(Entity entity, int? modifiedBy = null)
            where Entity : class, IEntity;

        void Delete<Entity>(object id)
            where Entity : class, IEntity;

        void Delete<Entity>(Entity entity)
            where Entity : class, IEntity;

        void Save();
    }
}
