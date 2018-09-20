using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class AboutViewModel
    {
        public IList<RecordLabel> WeCooperateWith { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The Email should be in example@example.com format!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
