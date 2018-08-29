using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class AuditableEntity : Entity, IAuditCreate, IAuditUpdate
    {
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
