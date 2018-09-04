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
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.SignUp, Type = EmailTemplates.SignUp, Name = "SignUp", Subject = "Account Activation", Body = "Dear [Name], <Br> You have registered an account on M Box. <br> [Link] <br><br>Regards, <br> M Box",LinkText = "Click here to verify your account." });
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.ForgotPassword, Type = EmailTemplates.ForgotPassword, Name = "ForgotPassword", Subject = "Forgot Password", Body = " Dear [Name], <br>You have requested a password change on M Box. <br>[Link]<br><br>If you did not request this change, please contact support in the About tab on M Box. <br><br>Regards, <br>M Box", LinkText = "Click here to change your password." });
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.DeletedRecordLabel, Type = EmailTemplates.DeletedRecordLabel, Name = "DeletedRecordLabel", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your M Box account has been terminated.<br> For more information, please contact us at [MBoxMail]<br><br>Regards,<br>M Box  " });
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.DeletedArtist, Type = EmailTemplates.DeletedArtist, Name = "DeletedArtist", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your record label has been removed. <br>Your page will remain as is, but you will not be able to post new songs until you join a record label that cooperates with us. <br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.InvitedRecordLabel, Type = EmailTemplates.InvitedRecordLabel, Name = "InvitedRecordLabel", Subject = "Create Your M Box Account", Body = "Dear [Name],<br>Your M Box account has been created! <br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.InvitedArtist, Type = EmailTemplates.InvitedArtist, Name = "InvitedArtist", Subject = "Create Your M Box Account", Body = "Dear [Name], <br>Your Record Label has invited you to join M Box. <br> M Box is a page where you can customize a page with all your music.<br> [Link]<br><br>Regards, <br>M Box", LinkText = "Click here to get started" });
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
                context.SaveChanges();

                var userReturned = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser.Id);
                recordLabel.User = userReturned;
                context.RecordLabels.Add(recordLabel);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser.Id, RoleId = 2 });

                var appUser1 = new ApplicationUser();
                var recordLabel1 = new RecordLabel();
                var passRL1 = new PasswordHasher<ApplicationUser>();
                var hashedRl1 = passRL.HashPassword(appUser1, "recordl!23");
                appUser1.Email = "testrl1@gmail.com";
                appUser1.Name = "testRecordLabel1";
                appUser1.IsActivated = true;
                appUser1.PasswordHash = hashedRl1;
                appUser1.NormalizedUserName = "TESTRL1@GMAIL.COM";
                appUser1.NormalizedEmail = "TESTRL1@GMAIL.COM";
                appUser1.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                appUser1.UserName = "testrl1@gmail.com";
                appUser1.LockoutEnabled = true;
                recordLabel1.User = appUser1;
                context.ApplicationUsers.Add(appUser1);
                context.SaveChanges();

                var userReturned1 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser1.Id);
                recordLabel1.User = userReturned1;
                context.RecordLabels.Add(recordLabel1);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser1.Id, RoleId = 2 });

                var appUser2 = new ApplicationUser();
                var recordLabel2 = new RecordLabel();
                var passRL2 = new PasswordHasher<ApplicationUser>();
                var hashedRl2 = passRL.HashPassword(appUser2, "recordl!23");
                appUser2.Email = "testrl2@gmail.com";
                appUser2.Name = "testRecordLabel2";
                appUser2.IsActivated = true;
                appUser2.PasswordHash = hashedRl2;
                appUser2.NormalizedUserName = "TESTRL2@GMAIL.COM";
                appUser2.NormalizedEmail = "TESTRL2@GMAIL.COM";
                appUser2.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                appUser2.UserName = "testrl2@gmail.com";
                appUser2.LockoutEnabled = true;
                recordLabel2.User = appUser2;
                context.ApplicationUsers.Add(appUser2);
                context.SaveChanges();

                var userReturned2 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser2.Id);
                recordLabel2.User = userReturned2;
                context.RecordLabels.Add(recordLabel2);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser2.Id, RoleId = 2 });

                var appUser3 = new ApplicationUser();
                var recordLabel3 = new RecordLabel();
                var passRL3 = new PasswordHasher<ApplicationUser>();
                var hashedRl3 = passRL.HashPassword(appUser3, "recordl!23");
                appUser3.Email = "testrl3@gmail.com";
                appUser3.Name = "testRecordLabel3";
                appUser3.IsActivated = true;
                appUser3.PasswordHash = hashedRl3;
                appUser3.NormalizedUserName = "TESTRL3@GMAIL.COM";
                appUser3.NormalizedEmail = "TESTRL3@GMAIL.COM";
                appUser3.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                appUser3.UserName = "testrl3@gmail.com";
                appUser3.LockoutEnabled = true;
                recordLabel3.User = appUser3;
                context.ApplicationUsers.Add(appUser3);
                context.SaveChanges();

                var userReturned3 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser3.Id);
                recordLabel3.User = userReturned3;
                context.RecordLabels.Add(recordLabel3);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser3.Id, RoleId = 2 });

                var appUser4 = new ApplicationUser();
                var recordLabel4 = new RecordLabel();
                var passRL4 = new PasswordHasher<ApplicationUser>();
                var hashedRl4 = passRL.HashPassword(appUser4, "recordl!23");
                appUser4.Email = "testrl4@gmail.com";
                appUser4.Name = "testRecordLabel4";
                appUser4.IsActivated = true;
                appUser4.PasswordHash = hashedRl4;
                appUser4.NormalizedUserName = "TESTRL4@GMAIL.COM";
                appUser4.NormalizedEmail = "TESTRL4@GMAIL.COM";
                appUser4.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                appUser4.UserName = "testrl4@gmail.com";
                appUser4.LockoutEnabled = true;
                recordLabel4.User = appUser4;
                context.ApplicationUsers.Add(appUser4);
                context.SaveChanges();

                var userReturned4 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser4.Id);
                recordLabel4.User = userReturned4;
                context.RecordLabels.Add(recordLabel4);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser4.Id, RoleId = 2 });

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
                context.ApplicationUsers.Add(appUser);
                context.SaveChanges();

                var userReturned = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser.Id);
                artist.User = userReturned;
                context.Artists.Add(artist);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser.Id, RoleId = 3 });

                var appUser1 = new ApplicationUser();
                var artist1 = new Artist();
                var passArtist1 = new PasswordHasher<ApplicationUser>();
                var hashedArtist1 = passArtist1.HashPassword(appUser1, "artist!23");
                appUser1.Email = "testartist1@gmail.com";
                appUser1.Name = "testArtist1";
                appUser1.IsActivated = true;
                appUser1.PasswordHash = hashedArtist1;
                appUser1.NormalizedEmail = "TESTARTIST1@GMAIL.COM";
                appUser1.NormalizedUserName = "TESTARTIST1@GMAIL.COM";
                appUser1.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser1.UserName = "testartist1@gmail.com";
                appUser1.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser1);
                context.SaveChanges();

                var userReturned1 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser1.Id);
                artist1.User = userReturned1;
                context.Artists.Add(artist1);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser1.Id, RoleId = 3 });

                var appUser2 = new ApplicationUser();
                var artist2 = new Artist();
                var passArtist2 = new PasswordHasher<ApplicationUser>();
                var hashedArtist2 = passArtist.HashPassword(appUser2, "artist!23");
                appUser2.Email = "testartist2@gmail.com";
                appUser2.Name = "testArtist2";
                appUser2.IsActivated = true;
                appUser2.PasswordHash = hashedArtist2;
                appUser2.NormalizedEmail = "TESTARTIST2@GMAIL.COM";
                appUser2.NormalizedUserName = "TESTARTIST2@GMAIL.COM";
                appUser2.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser2.UserName = "testartist2@gmail.com";
                appUser2.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser2);
                context.SaveChanges();

                var userReturned2 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser2.Id);
                artist2.User = userReturned2;
                context.Artists.Add(artist2);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser2.Id, RoleId = 3 });

                var appUser3 = new ApplicationUser();
                var artist3 = new Artist();
                var passArtist3 = new PasswordHasher<ApplicationUser>();
                var hashedArtist3 = passArtist.HashPassword(appUser3, "artist!23");
                appUser3.Email = "testartist3@gmail.com";
                appUser3.Name = "testArtist3";
                appUser3.IsActivated = true;
                appUser3.PasswordHash = hashedArtist3;
                appUser3.NormalizedEmail = "TESTARTIST3@GMAIL.COM";
                appUser3.NormalizedUserName = "TESTARTIST3@GMAIL.COM";
                appUser3.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser3.UserName = "testartist3@gmail.com";
                appUser3.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser3);
                context.SaveChanges();

                var userReturned3 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser3.Id);
                artist3.User = userReturned3;
                context.Artists.Add(artist3);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser3.Id, RoleId = 3 });

                var appUser4 = new ApplicationUser();
                var artist4 = new Artist();
                var passArtist4 = new PasswordHasher<ApplicationUser>();
                var hashedArtist4 = passArtist.HashPassword(appUser4, "artist!23");
                appUser4.Email = "testartist4@gmail.com";
                appUser4.Name = "testArtist4";
                appUser4.IsActivated = true;
                appUser4.PasswordHash = hashedArtist4;
                appUser4.NormalizedEmail = "TESTARTIST4@GMAIL.COM";
                appUser4.NormalizedUserName = "TESTARTIST4@GMAIL.COM";
                appUser4.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser4.UserName = "testartist4@gmail.com";
                appUser4.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser4);
                context.SaveChanges();

                var userReturned4 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser4.Id);
                artist4.User = userReturned4;
                context.Artists.Add(artist4);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser4.Id, RoleId = 3 });

                context.SaveChanges();
            }

            if (!context.Configurations.Any())
            {
                context.Configurations.Add(new Configuration() { Key = nameof(SourceEmails.ItLabsEmail), Value = "no-reply@it-labs.com" });
                context.SaveChanges();
            }

            if (!context.RecordLabelArtists.Any())
            {
                var rl = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testRecordLabel");
                var rl1 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testRecordLabel1");
                var rl2 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testRecordLabel2");
                var rl3 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testRecordLabel3");

                var ar = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testArtist");
                var ar1 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testArtist1");
                var ar2 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testArtist2");
                var ar3 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testArtist3");
                var ar4 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "testArtist4");

                var recordLabel = context.RecordLabels.FirstOrDefault(c => c.Id == rl.Id);
                var recordLabel1 = context.RecordLabels.FirstOrDefault(c => c.Id == rl1.Id);
                var recordLabel2 = context.RecordLabels.FirstOrDefault(c => c.Id == rl2.Id);
                var recordLabel3 = context.RecordLabels.FirstOrDefault(c => c.Id == rl3.Id);

                var artist = context.Artists.FirstOrDefault(c => c.Id == ar.Id);
                var artist1 = context.Artists.FirstOrDefault(c => c.Id == ar1.Id);
                var artist2 = context.Artists.FirstOrDefault(c => c.Id == ar2.Id);
                var artist3 = context.Artists.FirstOrDefault(c => c.Id == ar3.Id);
                var artist4 = context.Artists.FirstOrDefault(c => c.Id == ar4.Id);

                var rla = new RecordLabelArtists();
                rla.Artist = artist;
                rla.RecordLabel = recordLabel;
                context.RecordLabelArtists.Add(rla);

                var rla1 = new RecordLabelArtists();
                rla1.Artist = artist1;
                rla1.RecordLabel = recordLabel;
                context.RecordLabelArtists.Add(rla1);

                var rla2 = new RecordLabelArtists();
                rla2.Artist = artist2;
                rla2.RecordLabel = recordLabel1;
                context.RecordLabelArtists.Add(rla2);

                var rla3 = new RecordLabelArtists();
                rla3.Artist = artist3;
                rla3.RecordLabel = recordLabel2;
                context.RecordLabelArtists.Add(rla3);

                var rla4 = new RecordLabelArtists();
                rla4.Artist = artist4;
                rla4.RecordLabel = recordLabel3;
                context.RecordLabelArtists.Add(rla4);

                context.SaveChanges();
            }
        }
    }
}



