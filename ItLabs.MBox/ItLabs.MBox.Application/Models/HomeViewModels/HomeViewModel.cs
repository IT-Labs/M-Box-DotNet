using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Application.Models
{
    public class HomeViewModel
    {
        public IList<Song> RecentlyAddedSongs { get; set; }
        public IList<Artist> MostFollowedArtists { get; set; }
        public IList<Song> MostPopularArtistSongs { get; set; }
    }
}
