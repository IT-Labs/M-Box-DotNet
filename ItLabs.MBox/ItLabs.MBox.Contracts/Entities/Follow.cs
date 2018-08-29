using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Follow : IAuditable
    {
        public int Id { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual ApplicationUser Follower { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
