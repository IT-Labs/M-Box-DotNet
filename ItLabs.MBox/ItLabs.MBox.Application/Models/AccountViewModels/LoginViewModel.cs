using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="The Email should be in example@example.com format!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
