using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>, IEntity, IAuditCreate, IAuditUpdate
    {
        public virtual string Name { get; set; }

        public virtual bool IsActivated { get; set; }

        public virtual string Picture { get; set; }

        public virtual int ModifiedBy { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
    }
}
