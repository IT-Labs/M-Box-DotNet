using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.ArtistsViewModel
{
    public class AddNewSongViewModel
    {
        [Required(ErrorMessage = "Name must contain more than 2 alphanumeric characters!")]
        [Display(Name = "Song Name")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 2)]
        public string SongName { get; set; }

        [Required(ErrorMessage = "Name must contain more than 2 alphanumeric characters!")]
        [Display(Name = "Album Name")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters!", MinimumLength = 2)]
        public string AlbumName { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string YoutubeLink { get; set; }

        [Required]
        public string VimeoLink { get; set; }

        [Required]
        public Genres Genres { get; set; }

        [StringLength(10000, ErrorMessage = "The {0} cannot contain more than {1} characters!")]
        public string SongLyrics { get; set; }
    }
}
