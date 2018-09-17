using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models
{
    public class InviteViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "The Email should be in example@example.com format!")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 1)]
        public string Name { get; set; }
    }
}
