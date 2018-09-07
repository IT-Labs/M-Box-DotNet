using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Artist : AuditableEntity
    {
        public Artist()
        {
            RecordLabelArtists = new List<RecordLabelArtist>();
        }

        public virtual string Bio { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecordLabelArtist> RecordLabelArtists { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public string RecordLabelName => RecordLabelArtists?.FirstOrDefault()?.RecordLabel?.User.Name;
    }
}
