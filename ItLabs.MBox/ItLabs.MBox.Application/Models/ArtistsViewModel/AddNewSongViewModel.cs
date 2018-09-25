using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.ArtistsViewModel
{
    public class AddNewSongViewModel
    {
        [Required()]
        [Display(Name = "Song Name")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters, or less than {2}!", MinimumLength = 2)]
        public string SongName { get; set; }

        [Required]
        [Display(Name = "Album Name")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters, or less than {2}", MinimumLength = 2)]
        public string AlbumName { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please enter a valid YouTube link!")]
        [RegularExpression(@"^(http(s?)\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$", ErrorMessage = "Please enter a valid Youtube link!")]
        public string YoutubeLink { get; set; }

        [Required(ErrorMessage = "Please enter a valid Vimeo link!")]
        [RegularExpression(@"^(http(s?)\:\/\/)?(www\.)?vimeo.com\/(?:channels\/(?:\w+\/)?|groups\/([^\/]*)\/videos\/|)(\d+)(?:|\/\?)", ErrorMessage = "Please enter a valid Vimeo link!")]
        public string VimeoLink { get; set; }


        public Genres Genres { get; set; }

        [StringLength(10000, ErrorMessage = "The {0} cannot contain more than {1} characters!")]
        public string SongLyrics { get; set; }

        public Song Song { get; set; }
    }
}
