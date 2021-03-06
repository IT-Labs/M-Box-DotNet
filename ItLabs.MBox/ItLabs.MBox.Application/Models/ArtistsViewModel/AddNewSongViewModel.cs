﻿using ItLabs.MBox.Common.Attributes;
using ItLabs.MBox.Contracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ItLabs.MBox.Application.Models.ArtistsViewModel
{
    public class AddNewSongViewModel
    {
        public int SongId { get; set; }
        [Required()]
        [Display(Name = "Song Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters, or less than {2}!", MinimumLength = 2)]
        public string SongName { get; set; }

        public string Picture;

        [Required]
        [Display(Name = "Album Name")]
        [StringLength(50, ErrorMessage = "The {0} cannot contain more than {1} characters, or less than {2}", MinimumLength = 2)]
        public string AlbumName { get; set; }

        [Required(ErrorMessage = "Date of release is required field.")]
        [CurrentDate(ErrorMessage ="Cannot set date in future")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please enter a valid YouTube link!")]
        [RegularExpression(@"^(http(s?)\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$", ErrorMessage = "Please enter a valid Youtube link!")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters")]
        public string YoutubeLink { get; set; }

        [Required(ErrorMessage = "Please enter a valid Vimeo link!")]
        [RegularExpression(@"^(http(s?)\:\/\/)?(www\.)?vimeo.com\/(?:channels\/(?:\w+\/)?|groups\/([^\/]*)\/videos\/|)(\d+)(?:|\/\?)", ErrorMessage = "Please enter a valid Vimeo link!")]
        [StringLength(100, ErrorMessage = "The {0} cannot contain more than {1} characters")]
        public string VimeoLink { get; set; }


        public Genres Genres { get; set; }

        public string GenreName { get; set; }

        [StringLength(10000, ErrorMessage = "The {0} cannot contain more than {1} characters!")]
        public string SongLyrics { get; set; }

    }
}
