using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Artist : Entity
    {
        public Artist()
        {
            RecordLabelArtists = new List<RecordLabelArtist>();
            User = new ApplicationUser();
        }

        public virtual string Bio { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecordLabelArtist> RecordLabelArtists { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public string RecordLabelName => RecordLabelArtists?.FirstOrDefault()?.RecordLabel?.User.Name;

        public string PictureName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(User.Picture))
                {
                    return MBoxConstants.DefaultArtistImage;
                }
                return User.Picture;
            }
        }
    }
}
