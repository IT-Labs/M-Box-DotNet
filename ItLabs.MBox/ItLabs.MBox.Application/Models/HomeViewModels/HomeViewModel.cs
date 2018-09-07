using ItLabs.MBox.Contracts.Data_Transfer_Objects;
using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class HomeViewModel
    {
        public IList<Song> RecentlyAddedSongs { get; set; }
        public IList<Artist> MostFollowedArtists { get; set; }
        public IList<Song> RecentlyAddedSongsOfMostPopularArtist { get; set; }
    }
}
