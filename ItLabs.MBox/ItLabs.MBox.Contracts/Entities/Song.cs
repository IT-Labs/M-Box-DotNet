using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Song : IAuditable
    {
        public int SongId { get; set; }

        public string Name { get; set; }

        public string AlbumName { get; set; }

        public string Lyrics { get; set; }
        public string Image { get; set; }

        public DateTime DateOfRelease { get; set; }

        public string YoutubeLink { get; set; }

        public string VimeoLink { get; set; }

        public string Genre { get; set; }

        public virtual Artist Artist { get; set; }

        //public int ArtistId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
