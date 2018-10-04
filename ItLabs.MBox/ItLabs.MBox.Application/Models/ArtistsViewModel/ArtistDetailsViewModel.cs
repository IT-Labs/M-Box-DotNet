using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.ArtistsViewModel
{
    public class ArtistDetailsViewModel
    {
        public PagingModel<Song> PagingModelSongs { get; set; }
        public Artist Artist { get; set; }
        public int CurrentLoggedUserId { get; set; }

    }
}
