using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Data.Repositories
{
    public class ArtistsRepository : IArtistsRepository
    {
        private MBoxDbContext _MBoxContext;
        
        public ArtistsRepository(MBoxDbContext context)
        {
            _MBoxContext = context;
        }

        public IQueryable<Artist> GetAllArtists()
        {
            return _MBoxContext.Artists;

        }
    }
}
