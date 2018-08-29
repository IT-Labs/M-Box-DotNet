using System;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Song : IAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AlbumName { get; set; }

        public string Lyrics { get; set; }
        public string Picture { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string YouTubeLink { get; set; }

        public string VimeoLink { get; set; }

        public string Genre { get; set; }

        public virtual Artist Artist { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
