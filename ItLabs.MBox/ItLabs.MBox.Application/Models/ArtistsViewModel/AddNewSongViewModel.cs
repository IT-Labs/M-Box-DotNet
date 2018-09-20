using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.ArtistsViewModel
{
    public class AddNewSongViewModel
    {
        public string songName { get; set; }

        

        public Genres genres { get; set; }
    }
}
