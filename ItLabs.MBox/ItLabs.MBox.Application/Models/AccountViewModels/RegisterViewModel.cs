using ItLabs.MBox.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name must contain more than 2 alphanumeric characters!")]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email address should be in format example@exp.exp!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must contain at least {2} characters and 1 number or symbol! {0} cannot contain more than 64 characters!", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [PasswordValidation(Minimum = 8, Maximum = 64)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [PasswordValidation(Minimum = 8, Maximum = 64)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
