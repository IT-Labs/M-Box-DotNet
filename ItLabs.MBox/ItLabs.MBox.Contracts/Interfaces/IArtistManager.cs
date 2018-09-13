﻿using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistManager
    {
        IList<Artist> GetAllArtists();
        IList<Artist> GetMostFollowedArtists(int number);
        IList<Artist> GetAllUserArtists();
        IList<Artist> GetNextArtists(int skip, int take);
    }
}