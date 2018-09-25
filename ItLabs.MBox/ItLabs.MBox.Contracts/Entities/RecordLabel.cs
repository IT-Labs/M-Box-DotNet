using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabel : Entity
    { 
        public virtual string AboutInfo { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecordLabelArtist> RecordLabelArtists { get; set; }
        public string PictureName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(User.Picture))
                {
                    return MBoxConstants.DefaultRecordLabelImage;
                }
                return User.Picture;
            }
        }
    }
}
