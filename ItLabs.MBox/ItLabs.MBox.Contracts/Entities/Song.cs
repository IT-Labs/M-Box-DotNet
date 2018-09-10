using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Song : Entity
    {
        public virtual string Name { get; set; }

        public virtual string AlbumName { get; set; }

        public virtual string Lyrics { get; set; }
        public virtual string Picture { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        public virtual string YouTubeLink { get; set; }

        public virtual string VimeoLink { get; set; }

        public virtual string Genre { get; set; }

        public int ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
