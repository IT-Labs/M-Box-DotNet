using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Artist : AuditableEntity
    {
        public string Bio { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
