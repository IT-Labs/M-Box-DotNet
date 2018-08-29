using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>, IAuditCreate, IAuditUpdate
    {
        public string Name { get; set; }

        public Boolean IsActivated { get; set; }

        public string Picture { get; set; }

        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
