using ItLabs.MBox.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must contain at least {2} characters and 1 number or symbol", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [PasswordValidation(Minimum = 8, Maximum = 100)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
