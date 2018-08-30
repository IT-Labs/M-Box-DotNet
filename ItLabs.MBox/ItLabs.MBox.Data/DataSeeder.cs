using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.SignUp, Name = "SignUp", Subject = "Account Activation", Body = "Dear [Name], <Br> You have registered an account on M Box. <br> [Link] <br><br>Regards, <br> M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.ForgotPassword, Name = "ForgotPassword", Subject = "Forgot Password", Body = " Dear [Name], <br>You have requested a password change on M Box. <br>[Link]<br><br>If you did not request this change, please contact support in the About tab on M Box. <br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.DeletedRecordLabel, Name = "DeletedRecordLabel", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your M Box account has been terminated.<br> For more information, please contact us at [MBoxMail]<br><br>Regards,<br>M Box  " });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.DeletedArtist, Name = "DeletedArtist", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your record label has been removed. <br>Your page will remain as is, but you will not be able to post new songs until you join a record label that cooperates with us. <br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.InvitedRecordLabel, Name = "InvitedRecordLabel", Subject = "Create Your M Box Account", Body = "Dear [Name],<br>Your M Box account has been created! <br> [Link]<br><br>Regards, <br>M Box" });
                context.EmailTemplates.Add(new EmailTemplate() { Type = EmailTemplates.InvitedArtist, Name = "InvitedArtist", Subject = "Create Your M Box Account", Body = "Dear [Name], <br>Your Record Label has invited you to join M Box. <br> M Box is a page where you can customize a page with all your music.<br> [Link]<br><br>Regards, <br>M Box" });
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Admin, Id = (int)Roles.Admin, Name = Roles.Admin.ToString() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.RecordLabel, Id = (int)Roles.RecordLabel, Name = Roles.RecordLabel.ToString() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Artist, Id = (int)Roles.Artist, Name = Roles.Artist.ToString() });
                context.ApplicationRoles.Add(new ApplicationRole() { Type = Roles.Listener, Id = (int)Roles.Listener, Name = Roles.Listener.ToString() });
                context.SaveChanges();
            }
        }
    }
}



