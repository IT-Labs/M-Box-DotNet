using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace ItLabs.MBox.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MBoxDbContext>();
            context.Database.EnsureCreated();
            if (!context.EmailTemplates.Any())
            {
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.SignUp, Name = "SignUp", Subject = "Account Activation", Body = "Dear [Name], <Br> You have registered an account on M Box. <br> [Link] <br><br>Regards, <br> M Box",LinkText = "Click here to verify your account." });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.ForgotPassword, Name = "ForgotPassword", Subject = "Forgot Password", Body = " Dear [Name], <br>You have requested a password change on M Box. <br>[Link]<br><br>If you did not request this change, please contact support in the About tab on M Box. <br><br>Regards, <br>M Box", LinkText = "Click here to change your password." });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.DeletedRecordLabel, Name = "DeletedRecordLabel", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your M Box account has been terminated.<br> For more information, please contact us at [MBoxMail]<br><br>Regards,<br>M Box  " });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.DeletedArtist, Name = "DeletedArtist", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your record label has been removed. <br>Your page will remain as is, but you will not be able to post new songs until you join a record label that cooperates with us. <br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.InvitedRecordLabel, Name = "InvitedRecordLabel", Subject = "Create Your M Box Account", Body = "Dear [Name],<br>Your M Box account has been created! <br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.InvitedArtist, Name = "InvitedArtist", Subject = "Create Your M Box Account", Body = "Dear [Name], <br>Your Record Label has invited you to join M Box. <br> M Box is a page where you can customize a page with all your music.<br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Admin, Id = (int)Roles.Admin, Name = Roles.Admin.ToString(), NormalizedName = Roles.Admin.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.RecordLabel, Id = (int)Roles.RecordLabel, Name = Roles.RecordLabel.ToString(), NormalizedName = Roles.RecordLabel.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Artist, Id = (int)Roles.Artist, Name = Roles.Artist.ToString(), NormalizedName = Roles.Artist.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Listener, Id = (int)Roles.Listener, Name = Roles.Listener.ToString(), NormalizedName = Roles.Listener.ToString().ToUpper() });
                context.SaveChanges();
            }

            if (!context.ApplicationUsers.Any())
            {
                var admin = new ApplicationUser();
                var passAdmin = new PasswordHasher<ApplicationUser>();
                var hashedAdmin = passAdmin.HashPassword(admin, "admin!23");
                admin.Email = "testadmin@gmail.com";
                admin.Name = "testAdmin";
                admin.IsActivated = true;
                admin.NormalizedEmail = "TESTADMIN@GMAIL.COM";
                admin.NormalizedUserName = "TESTADMIN@GMAIL.COM";
                admin.SecurityStamp = "be0aefcf-bf66-40b7-a3a6-d42f58ef0beb";
                admin.LockoutEnabled = true;
                admin.UserName = "testadmin@gmail.com";
                admin.PasswordHash = hashedAdmin;
                context.ApplicationUsers.Add(admin);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = admin.Id, RoleId = 1});

                var listener = new ApplicationUser();
                var passListener = new PasswordHasher<ApplicationUser>();
                var hashedListenr = passListener.HashPassword(listener, "listener!23");
                listener.Email = "testlistener@gmail.com";
                listener.Name = "testListenr";
                listener.IsActivated = true;
                listener.NormalizedUserName = "TESTLISTENER@GMAIL.COM";
                listener.NormalizedEmail = "TESTLISTENER@GMAIL.COM";
                listener.SecurityStamp = "09715aaa-f1f3-4ced-a47d-dcb7588137ac";
                listener.UserName = "testlistener@gmail.com";
                listener.LockoutEnabled = true;
                listener.PasswordHash = hashedListenr;
                context.ApplicationUsers.Add(listener);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = listener.Id, RoleId = 4 });

                context.SaveChanges();
            }

            if (!context.Artists.Any())
            {
                var appUser = new ApplicationUser();
                var artist = new Artist();
                var passArtist = new PasswordHasher<ApplicationUser>();
                var hashedArtist = passArtist.HashPassword(appUser, "artist!23");
                appUser.Email = "testartist@gmail.com";
                appUser.Name = "testArtist";
                appUser.IsActivated = true;
                appUser.PasswordHash = hashedArtist;
                appUser.NormalizedEmail = "TESTARTIST@GMAIL.COM";
                appUser.NormalizedUserName = "TESTARTIST@GMAIL.COM";
                appUser.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser.UserName = "testartist@gmail.com";
                appUser.LockoutEnabled = true;
                artist.User = appUser;
                context.ApplicationUsers.Add(appUser);
                context.Artists.Add(artist);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser.Id, RoleId = 3 });

                context.SaveChanges();
            }

            if (!context.RecordLabels.Any())
            {
                var appUser = new ApplicationUser();
                var recordLabel = new RecordLabel();
                var passRL = new PasswordHasher<ApplicationUser>();
                var hashedRl = passRL.HashPassword(appUser, "recordl!23");
                appUser.Email = "testrl@gmail.com";
                appUser.Name = "testRecordLabel";
                appUser.IsActivated = true;
                appUser.PasswordHash = hashedRl;
                appUser.NormalizedUserName = "TESTRL@GMAIL.COM";
                appUser.NormalizedEmail = "TESTRL@GMAIL.COM";
                appUser.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                appUser.UserName = "testrl@gmail.com";
                appUser.LockoutEnabled = true;
                recordLabel.User = appUser;
                context.ApplicationUsers.Add(appUser);
                context.RecordLabels.Add(recordLabel);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser.Id, RoleId = 2 });

                context.SaveChanges();
            }
        }
    }
}



