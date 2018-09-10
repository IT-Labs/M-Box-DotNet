using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface ISongManager
    {
        IList<Song> GetRecentlyAddedSongs(int number);
        IList<Song> GetRecentlyAddedSongsOfMostPopularArtist(int number);
    }
}
