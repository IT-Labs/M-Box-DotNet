using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabel : AuditableEntity
    { 
        public string AboutInfo { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
