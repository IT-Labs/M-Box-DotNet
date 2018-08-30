using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IArtistsRepository
    {
        IQueryable<Artist> GetAllArtists();
    }
}
