using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabelArtists : IAuditCreate, IAuditUpdate
    {
        public virtual ApplicationUser Id { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual RecordLabel RecordLabel { get; set; }

        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
    }
}
