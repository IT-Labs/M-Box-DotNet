using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ItLabs.MBox.Contracts.Entities;

namespace ItLabs.MBox.Data
{
    public class DataSeeder
    {
        public static void Seed()
        {
            MBoxDbContext context = new MBoxDbContext(new DbContextOptions<MBoxDbContext>());
            context.ApplicationUserRoles.Add(new ApplicationUserRoles((int)RolesEnum.ADMIN , RolesEnum.ADMIN.ToString()));
            context.ApplicationUserRoles.Add(new ApplicationUserRoles((int)RolesEnum.RECORDLABEL, RolesEnum.RECORDLABEL.ToString()));
            context.ApplicationUserRoles.Add(new ApplicationUserRoles((int)RolesEnum.ARTIST, RolesEnum.ARTIST.ToString()));
            context.ApplicationUserRoles.Add(new ApplicationUserRoles((int)RolesEnum.LISTENER, RolesEnum.LISTENER.ToString()));
            context.SaveChanges();
        }
    }
}
