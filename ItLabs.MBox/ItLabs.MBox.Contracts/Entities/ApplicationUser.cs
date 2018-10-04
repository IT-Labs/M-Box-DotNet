using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Contracts.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public ApplicationUser() : base()
        {
            Follows = new List<Follow>();
        }
        public virtual string Name { get; set; }

        public virtual bool IsActivated { get; set; }

        public virtual string Picture { get; set; }

        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public string PictureName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Picture))
                {
                    return MBoxConstants.DefaultArtistImage;
                }
                return Picture;
            }
        }
        public bool IsFollowing(int artistId)
        {
            return Follows.Select(x => x.Artist?.Id).Contains(artistId);
        }
    }
}
