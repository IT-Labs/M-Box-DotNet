using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
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
            
            if (!context.ApplicationUsers.Any())
            {
                var admin = new ApplicationUser();
                var passAdmin = new PasswordHasher<ApplicationUser>();
                var hashedAdmin = passAdmin.HashPassword(admin, "admin!23");
                admin.Id = (int)Role.SuperAdmin;
                admin.Email = "superadmin@gmail.com";
                admin.Name = "Super Admin";
                admin.IsActivated = true;
                admin.NormalizedEmail = admin.Email.ToUpper();
                admin.NormalizedUserName = admin.Email.ToUpper();
                admin.SecurityStamp = "be0aefcf-bf66-40b7-a3a6-d42f58ef0beb";
                admin.LockoutEnabled = true;
                admin.UserName = admin.Email;
                admin.PasswordHash = hashedAdmin;
                admin.CreatedBy = (int)Role.SuperAdmin;
                admin.EmailConfirmed = true;
                context.ApplicationUsers.Add(admin);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = admin.Id, RoleId = 1});

                
            }
            if (!context.Roles.Any())
            {
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.SuperAdmin, Id = (int)Role.SuperAdmin, Name = Role.SuperAdmin.ToString(), NormalizedName = Role.SuperAdmin.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.RecordLabel, Id = (int)Role.RecordLabel, Name = Role.RecordLabel.ToString(), NormalizedName = Role.RecordLabel.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.Artist, Id = (int)Role.Artist, Name = Role.Artist.ToString(), NormalizedName = Role.Artist.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.Listener, Id = (int)Role.Listener, Name = Role.Listener.ToString(), NormalizedName = Role.Listener.ToString().ToUpper() });

                
            }
            if (!context.EmailTemplates.Any())
            {
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.SignUp, Type = EmailTemplateType.SignUp, Name = "SignUp", Subject = "Account Activation", Body = "Dear [Name], <Br> You have registered an account on M Box. <br> [Link] <br><br>Regards, <br> M Box", LinkText = "Click here to verify your account." });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.ForgotPassword, Type = EmailTemplateType.ForgotPassword, Name = "ForgotPassword", Subject = "Forgot Password", Body = " Dear [Name], <br>You have requested a password change on M Box. <br>[Link]<br><br>If you did not request this change, please contact support in the About tab on M Box. <br><br>Regards, <br>M Box", LinkText = "Click here to change your password." });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.DeletedRecordLabel, Type = EmailTemplateType.DeletedRecordLabel, Name = "DeletedRecordLabel", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your M Box account has been terminated.<br> For more information, please contact us at [MBoxMail]<br><br>Regards,<br>M Box  " });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.DeletedArtist, Type = EmailTemplateType.DeletedArtist, Name = "DeletedArtist", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your record label has been removed. <br>Your page will remain as is, but you will not be able to post new songs until you join a record label that cooperates with us. <br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.InvitedRecordLabel, Type = EmailTemplateType.InvitedRecordLabel, Name = "InvitedRecordLabel", Subject = "Create Your M Box Account", Body = "Dear [Name],<br>Your M Box account has been created! <br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.InvitedArtist, Type = EmailTemplateType.InvitedArtist, Name = "InvitedArtist", Subject = "Create Your M Box Account", Body = "Dear [Name], <br>Your Record Label has invited you to join M Box. <br> M Box is a page where you can customize a page with all your music.<br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
                context.EmailTemplates.Add(new EmailTemplate() { CreatedBy = (int)Role.SuperAdmin, Id = (int)EmailTemplateType.ContactForm, Type = EmailTemplateType.ContactForm, Name = "ContactForm", Subject = "M Box About Page Mail" });

                
            }
            context.SaveChanges();
            //if (!context.RecordLabels.Any())
            //{
            //    var recordLabel = new ApplicationUser();
            //    var passAdmin = new PasswordHasher<ApplicationUser>();
            //    recordLabel.Id = (int)Role.RecordLabel;
            //    recordLabel.Email = "testrl@gmail.com";
            //    recordLabel.Name = "Record Label";
            //    recordLabel.IsActivated = true;
            //    recordLabel.NormalizedEmail = recordLabel.Email.ToUpper();
            //    recordLabel.NormalizedUserName = recordLabel.Email.ToUpper();
            //    recordLabel.SecurityStamp = "be0aefcf-bf66-40b7-a3a6-d42f58ef0beb";
            //    recordLabel.LockoutEnabled = true;
            //    recordLabel.UserName = recordLabel.Email;
            //    recordLabel.PasswordHash = passAdmin.HashPassword(recordLabel, "record!23");
            //    recordLabel.CreatedBy = (int)Role.SuperAdmin;
            //    recordLabel.EmailConfirmed = true;
            //    context.ApplicationUsers.Add(recordLabel);
            //    context.UserRoles.Add(new IdentityUserRole<int>() { UserId = recordLabel.Id, RoleId = 1 });

            //    context.SaveChanges();
            //}
        }
    }
}



