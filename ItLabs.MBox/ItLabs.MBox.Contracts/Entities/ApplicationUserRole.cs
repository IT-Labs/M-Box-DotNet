using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class ApplicationUserRole : IdentityRole<int>
    {
        public virtual RolesEnum Role { get; set; }


        public ApplicationUserRole(RolesEnum inputRole): base(inputRole.ToString())
        {
            Id = (int)inputRole;
            Role = inputRole;
        }

}
}
