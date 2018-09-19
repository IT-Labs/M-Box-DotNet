using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ItLabs.MBox.Common.Extentions
{
    public static class UserManagerExtentions
    {
        public static Task<ApplicationUser> CreateUser(this UserManager<ApplicationUser> userManager, string name, string email, Role role, string password = null)
        {
            if (password == null)
                password = Guid.NewGuid().ToString();

            var user = new ApplicationUser { Name = name, UserName = email, Email = email };
            if(userManager.FindByEmailAsync(email).Result != null)
            {
                return null;
            }
            var result = userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {
                return null;
            }
            var roleResult = userManager.AddToRoleAsync(user, role.ToString()).Result;

            if (!roleResult.Succeeded)
            {
                return null;
            }

            return Task.FromResult(user);

        }
    }
}
