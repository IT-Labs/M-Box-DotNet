﻿using ItLabs.MBox.Contracts.Data_Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must contain at least 8 characters and 1 number or symbol", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [PasswordValidation(Minimum =8, Maximum =64)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [PasswordValidation(Minimum = 8, Maximum = 64)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}