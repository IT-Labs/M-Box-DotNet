using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Contracts.Entities
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int> , IAuditable
    {
        //public int ApplicationUserId { get; set; }

        public string Name { get; set; }

        public Boolean IsActivated { get; set; }

        public virtual ApplicationUserRoles ApplicationUserRole { get; set; }

        public string Picture { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
