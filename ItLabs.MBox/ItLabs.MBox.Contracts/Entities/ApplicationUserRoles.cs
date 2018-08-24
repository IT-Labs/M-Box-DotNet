using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class ApplicationUserRoles : IdentityRole<int>
    {
        public virtual RolesEnum Role { get; set; }


        public ApplicationUserRoles(int roleId, string roleName): base(roleName)
        {
            Id = roleId;
            Role = (RolesEnum)roleId;
        }

}
}
