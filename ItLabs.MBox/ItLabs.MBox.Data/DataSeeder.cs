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

                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.SuperAdmin, Id = (int)Role.SuperAdmin, Name = Role.SuperAdmin.ToString(), NormalizedName = Role.SuperAdmin.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.RecordLabel, Id = (int)Role.RecordLabel, Name = Role.RecordLabel.ToString(), NormalizedName = Role.RecordLabel.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.Artist, Id = (int)Role.Artist, Name = Role.Artist.ToString(), NormalizedName = Role.Artist.ToString().ToUpper() });
                context.ApplicationRoles.Add(new ApplicationRole() { CreatedBy = (int)Role.SuperAdmin, Type = Role.Listener, Id = (int)Role.Listener, Name = Role.Listener.ToString(), NormalizedName = Role.Listener.ToString().ToUpper() });

                context.SaveChanges();
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

                context.SaveChanges();
            }
            if (!context.RecordLabels.Any())
            {
                for (int i = 1; i < 250; i++)
                {
                    var tempUser = new ApplicationUser();
                    var tempRl = new RecordLabel();
                    var tempPass = new PasswordHasher<ApplicationUser>();
                    var hashedTempPass = tempPass.HashPassword(tempUser, "recordl!23" + i);
                    tempUser.Email = "testrl" + i + "@gmail.com";
                    tempUser.Name = "Record Label " + i;
                    tempUser.IsActivated = true;
                    tempUser.PasswordHash = hashedTempPass;
                    tempUser.NormalizedUserName = "TESTRL" + i + "@GMAIL.COM";
                    tempUser.NormalizedEmail = "TESTRL" + i + "@GMAIL.COM";
                    tempUser.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                    tempUser.UserName = "testrl" + i + "@gmail.com";
                    tempUser.LockoutEnabled = true;
                    tempRl.User = tempUser;
                    context.ApplicationUsers.Add(tempUser);
                    context.SaveChanges();

                    var tempReturned = context.ApplicationUsers.FirstOrDefault(c => c.Id == tempUser.Id);
                    tempRl.User = tempReturned;
                    context.RecordLabels.Add(tempRl);
                    context.UserRoles.Add(new IdentityUserRole<int>() { UserId = tempUser.Id, RoleId = (int)Role.RecordLabel });
                    
                    for(int j = 250; j < 255; j++)
                    {
                        var tempUserArtist = new ApplicationUser();
                        var tempArtist = new Artist();
                        var tempPassArtist = new PasswordHasher<ApplicationUser>();
                        var hashedTempPassArtist = tempPassArtist.HashPassword(tempUserArtist, "artist!23" + (i+j));
                        tempUserArtist.Email = "testArtist" + (i+j) + "@gmail.com";
                        tempUserArtist.Name = "Artist " + (i + j);
                        tempUserArtist.IsActivated = true;
                        tempUserArtist.PasswordHash = hashedTempPassArtist;
                        tempUserArtist.NormalizedUserName = "testArtist" +( i + j) + "@GMAIL.COM";
                        tempUserArtist.NormalizedEmail = "testArtist" +( i + j) + "@GMAIL.COM";
                        tempUserArtist.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                        tempUserArtist.UserName = "testArtist" + (i + j) + "@gmail.com";
                        tempUserArtist.LockoutEnabled = true;
                        tempArtist.User = tempUserArtist;
                        context.ApplicationUsers.Add(tempUserArtist);
                        context.SaveChanges();

                        var tempReturnedArtist = context.ApplicationUsers.FirstOrDefault(c => c.Id == tempUserArtist.Id);
                        tempArtist.User = tempReturnedArtist;
                        context.Artists.Add(tempArtist);
                        context.UserRoles.Add(new IdentityUserRole<int>() { UserId = tempUserArtist.Id, RoleId = (int)Role.Artist });
                        context.RecordLabelArtists.Add(new RecordLabelArtist() {RecordLabel = tempRl,DateCreated=DateTime.UtcNow, CreatedBy=  1, ModifiedBy = 1,Artist= tempArtist });
                    }

                }

                context.SaveChanges();
            }

        }
    }
}



