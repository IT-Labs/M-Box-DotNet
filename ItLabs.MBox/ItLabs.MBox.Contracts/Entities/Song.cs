using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Song : AuditableEntity
    {
        public string Name { get; set; }

        public string AlbumName { get; set; }

        public string Lyrics { get; set; }
        public string Picture { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string YouTubeLink { get; set; }

        public string VimeoLink { get; set; }

        public string Genre { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
