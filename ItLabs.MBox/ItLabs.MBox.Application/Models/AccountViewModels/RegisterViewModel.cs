using ItLabs.MBox.Contracts.Data_Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "The Email should be in example@example.com format!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must contain at least 8 characters and 1 number or symbol", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [PasswordValidation(Minimum = 8, Maximum = 100)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [PasswordValidation(Minimum = 8, Maximum = 100)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
