﻿using ItLabs.MBox.Contracts.Enums;
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

        public string PictureName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Picture))
                {
                    return MBoxConstants.DefaultSongImage;
                }
                return Picture;
            }
        }
        public string ShortName
        {
            get
            {
                if (Name.Length > 16)
                {
                    return Name.Substring(0, 16);
                }
                return Name;
            }
        }
    }
}
