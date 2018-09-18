﻿using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistManager : IBaseManager<Artist>
    {
        IList<Artist> GetMostFollowedArtists(int number);
        IList<Artist> GetRecordLabelArtists(int recordLabelId, int skip, int take);
        void AddArtistToRecordLabel(Artist artist, RecordLabel recordLabel);
        IList<Artist> GetSearchedArtists(int recordLabelId, int skip, int take, string search);
        
    }
}