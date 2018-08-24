using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Follow : IAuditable
    {
        public int FollowId { get; set; }

        public virtual Artist Artist { get; set; }
        //public int ArtistId { get; set; }

        public virtual ApplicationUser Follower { get; set; }
        //public int FollowerId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
