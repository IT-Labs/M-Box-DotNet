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
                context.EmailTemplates.Add(new EmailTemplate() { Id = (int)EmailTemplates.ContactForm, Type = EmailTemplates.ContactForm, Name = "ContactForm", Subject = "M Box About Page Mail" });

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
                appUser1.Name = "Top Dawg Entertainment";
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
                appUser2.Name = "Dreamville Records";
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
                appUser3.Name = "Glassnote Records";
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

                for(int i = 50; i < 150; i++)
                {
                    var tempUser = new ApplicationUser();
                    var tempRl = new RecordLabel();
                    var tempPass = new PasswordHasher<ApplicationUser>();
                    var hashedTempPass = passRL.HashPassword(tempUser, "recordl!23"+i);
                    tempUser.Email = "testrl"+i+"@gmail.com";
                    tempUser.Name = "Record Label " + i;
                    tempUser.IsActivated = true;
                    tempUser.PasswordHash = hashedTempPass;
                    tempUser.NormalizedUserName = "TESTRL"+i+"@GMAIL.COM";
                    tempUser.NormalizedEmail = "TESTRL"+i+"@GMAIL.COM";
                    tempUser.SecurityStamp = "415bf8f4-bc79-4ec2-8368-cf9bdd755db1";
                    tempUser.UserName = "testrl"+i+"@gmail.com";
                    tempUser.LockoutEnabled = true;
                    tempRl.User = tempUser;
                    context.ApplicationUsers.Add(tempUser);
                    context.SaveChanges();

                    var tempReturned = context.ApplicationUsers.FirstOrDefault(c => c.Id == tempUser.Id);
                    tempRl.User = tempReturned;
                    context.RecordLabels.Add(tempRl);
                    context.UserRoles.Add(new IdentityUserRole<int>() { UserId = tempUser.Id, RoleId = 2 });

                }
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
                appUser2.Name = "Smino";
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
                appUser3.Name = "Childish Gambino";
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
                appUser4.Name = "Ab-Soul";
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


                var appUser5 = new ApplicationUser();
                var artist5 = new Artist();
                var passArtist5 = new PasswordHasher<ApplicationUser>();
                var hashedArtist5 = passArtist.HashPassword(appUser5, "artist!23");
                appUser5.Email = "testartist5@gmail.com";
                appUser5.Name = "Kendrick Lamar";
                appUser5.IsActivated = true;
                appUser5.PasswordHash = hashedArtist5;
                appUser5.NormalizedEmail = "TESTARTIST5@GMAIL.COM";
                appUser5.NormalizedUserName = "TESTARTIST5@GMAIL.COM";
                appUser5.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser5.UserName = "testartist5@gmail.com";
                appUser5.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser5);
                context.SaveChanges();

                var userReturned5 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser5.Id);
                artist5.User = userReturned5;
                context.Artists.Add(artist5);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser5.Id, RoleId = 3 });

                var appUser6 = new ApplicationUser();
                var artist6 = new Artist();
                var passArtist6 = new PasswordHasher<ApplicationUser>();
                var hashedArtist6 = passArtist.HashPassword(appUser6, "artist!23");
                appUser6.Email = "testartist6@gmail.com";
                appUser6.Name = "J Cole";
                appUser6.IsActivated = true;
                appUser6.PasswordHash = hashedArtist6;
                appUser6.NormalizedEmail = "TESTARTIST6@GMAIL.COM";
                appUser6.NormalizedUserName = "TESTARTIST6@GMAIL.COM";
                appUser6.SecurityStamp = "e9279fdf-dee0-41b1-88f2-bf5c7508c220";
                appUser6.UserName = "testartist6@gmail.com";
                appUser6.LockoutEnabled = true;
                context.ApplicationUsers.Add(appUser6);
                context.SaveChanges();

                var userReturned6 = context.ApplicationUsers.FirstOrDefault(c => c.Id == appUser6.Id);
                artist6.User = userReturned6;
                context.Artists.Add(artist6);
                context.UserRoles.Add(new IdentityUserRole<int>() { UserId = appUser6.Id, RoleId = 3 });






                context.Songs.Add(new Song() { Artist = artist5, Name = "LOYALTY.", AlbumName = "DAMN.", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=Dlh-dzB2U4Y", VimeoLink = "https://vimeo.com/280033966", ReleaseDate = new DateTime(2017,4,14)});
                context.Songs.Add(new Song() { Artist = artist5, Name = "These Walls", AlbumName = "To Pimp a Butterfly", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=drV0QatqbRU", VimeoLink = "https://vimeo.com/109618785", ReleaseDate = new DateTime(2015, 2, 4) });
                context.Songs.Add(new Song() { Artist=artist6, Name = "Higher", AlbumName = "Friday Night Lights", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=hGIeg6EkPGE", VimeoLink = "https://vimeo.com/16821039", ReleaseDate = new DateTime(2010, 2, 12) });
                context.Songs.Add(new Song() { Artist= artist6, Name = "Change", AlbumName = "For Your Eyez Only", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=AiZuT69qJLc", VimeoLink = "https://vimeo.com/229542292", ReleaseDate = new DateTime(2016, 4, 14) });

                context.Songs.Add(new Song() { Artist = artist4, Name = "Terrorist Threats", AlbumName = "Control System", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=m_71q5lVEjc", VimeoLink = "https://vimeo.com/41895297", ReleaseDate = new DateTime(2003, 2, 24) });

                context.Songs.Add(new Song() { Artist = artist3, Name = "This is America", AlbumName = "No Album", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=VYOjWnS4cMY", VimeoLink = "https://vimeo.com/276794240", ReleaseDate = new DateTime(2018, 4, 14) });
                context.Songs.Add(new Song() { Artist = artist2, Name = "Amphetamine", AlbumName = "Blkswn", Genre = "Hip Hop", YouTubeLink = "https://www.youtube.com/watch?v=oRt-hqs_nrQ", VimeoLink = "https://vimeo.com/209110010", ReleaseDate = new DateTime(2014, 4, 2) });




                context.Follows.Add(new Follow() { Artist = artist3, Follower = userReturned5 });

                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned });
                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned1 });
                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned2 });
                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned3 });
                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned4 });
                context.Follows.Add(new Follow() { Artist = artist5, Follower = userReturned5 });


                context.Follows.Add(new Follow() { Artist = artist4, Follower = userReturned4 });
                context.Follows.Add(new Follow() { Artist = artist4, Follower = userReturned5 });

                context.Follows.Add(new Follow() { Artist = artist6, Follower = userReturned3 });
                context.Follows.Add(new Follow() { Artist = artist6, Follower = userReturned4 });
                context.Follows.Add(new Follow() { Artist = artist6, Follower = userReturned5 });


                context.SaveChanges();
            }

            if (!context.Configurations.Any())
            {
                context.Configurations.Add(new Configuration() { Key = nameof(SourceEmails.ItLabsEmail), Value = "no-reply@it-labs.com" });
                context.SaveChanges();
            }

            if (!context.RecordLabelArtists.Any())
            {
                var rl = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Top Dawg Entertainment");
                var rl1 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Dreamville Records");
                var rl3 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Glassnote Records");

                var ar = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Kendrick Lamar");
                var ar1 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "J Cole");
                var ar2 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Ab-Soul");
                var ar3 = context.ApplicationUsers.FirstOrDefault(c => c.Name == "Childish Gambino");


                var recordLabel = context.RecordLabels.FirstOrDefault(c => c.Id == rl.Id);
                var recordLabel1 = context.RecordLabels.FirstOrDefault(c => c.Id == rl1.Id);
                var recordLabel3 = context.RecordLabels.FirstOrDefault(c => c.Id == rl3.Id);

                var artist = context.Artists.FirstOrDefault(c => c.Id == ar.Id);
                var artist1 = context.Artists.FirstOrDefault(c => c.Id == ar1.Id);
                var artist2 = context.Artists.FirstOrDefault(c => c.Id == ar2.Id);
                var artist3 = context.Artists.FirstOrDefault(c => c.Id == ar3.Id);

                var rla = new RecordLabelArtist();
                rla.Artist = artist;
                rla.RecordLabel = recordLabel;
                context.RecordLabelArtists.Add(rla);

                var rla1 = new RecordLabelArtist();
                rla1.Artist = artist1;
                rla1.RecordLabel = recordLabel1;
                context.RecordLabelArtists.Add(rla1);

                var rla2 = new RecordLabelArtist();
                rla2.Artist = artist2;
                rla2.RecordLabel = recordLabel;
                context.RecordLabelArtists.Add(rla2);


                var rla4 = new RecordLabelArtist();
                rla4.Artist = artist3;
                rla4.RecordLabel = recordLabel3;
                context.RecordLabelArtists.Add(rla4);



                context.SaveChanges();
            }
        }
    }
}



