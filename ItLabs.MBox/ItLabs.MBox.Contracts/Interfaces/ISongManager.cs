using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface ISongManager : IBaseManager<Song>
    {
        IList<Song> GetRecentlyAddedSongs(int number);
        IList<Song> GetMostPopularArtistSongs(int number);
        IList<Song> GetArtistSongs(int ArtistId, int skip, int take, string searchValue);
    }
}
