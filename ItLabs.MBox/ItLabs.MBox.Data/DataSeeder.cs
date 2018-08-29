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
            //context.ApplicationUserRoles.Add(new ApplicationUserRole(Roles.ADMIN));
            //context.ApplicationUserRoles.Add(new ApplicationUserRole(Roles.RECORDLABEL));
            //context.ApplicationUserRoles.Add(new ApplicationUserRole(Roles.ARTIST));
            //context.ApplicationUserRoles.Add(new ApplicationUserRole(Roles.LISTENER));

            context.EmailTemplates.Add(new EmailTemplate() { Name = "SignUpTemplate", Subject = "Account Activation", Body = "Dear [Name], <Br> You have registered an account on M Box. <br> [Link] <br><br>Regards, <br> M Box" });
            context.EmailTemplates.Add(new EmailTemplate() { Name = "ForgotPasswordTemplate", Subject = "Forgot Password", Body = " Dear [Name], <br>You have requested a password change on M Box. <br>[Link]<br><br>If you did not request this change, please contact support in the About tab on M Box. <br><br>Regards, <br>M Box" });
            context.EmailTemplates.Add(new EmailTemplate() { Name = "DeletedRecordLabel", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your M Box account has been terminated.<br> For more information, please contact us at [MBoxMail]<br><br>Regards,<br>M Box  " });
            context.EmailTemplates.Add(new EmailTemplate() { Name = "DeletedArtist", Subject = "M Box Account Terminated", Body = "Dear [Name], <br>Your record label has been removed. <br>Your page will remain as is, but you will not be able to post new songs until you join a record label that cooperates with us. <br><br>Regards, <br>M Box" });
            context.EmailTemplates.Add(new EmailTemplate() { Name = "InvitedRecordLabel", Subject = "Create Your M Box Account", Body = "Dear [Name],<br>Your M Box account has been created! <br> [Link]<br><br>Regards, <br>M Box" });
            context.EmailTemplates.Add(new EmailTemplate() { Name = "InvitedArtist", Subject = "Create Your M Box Account", Body = "Dear [Name], <br>Your Record Label has invited you to join M Box. <br> M Box is a page where you can customize a page with all your music.<br> [Link]<br><br>Regards, <br>M Box" });


            context.SaveChanges();
        }
    }
}
