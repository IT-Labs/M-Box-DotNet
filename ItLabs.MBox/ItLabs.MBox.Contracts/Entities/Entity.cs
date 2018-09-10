using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}
