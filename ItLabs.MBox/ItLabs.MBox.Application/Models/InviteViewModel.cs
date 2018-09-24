using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models
{
    public class InviteViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address should be in format example@exp.exp!")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Email address should be in format example@exp.exp!")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
