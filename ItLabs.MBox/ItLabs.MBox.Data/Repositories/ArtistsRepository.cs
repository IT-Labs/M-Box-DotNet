using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ItLabs.MBox.Data.Repositories
{
    public class ArtistsRepository
    {
        private MBoxDbContext _MBoxContext;
        
        public ArtistsRepository()
        {
            _MBoxContext = new MBoxDbContext(new DbContextOptions<MBoxDbContext>());
        }

        public IQueryable<Artist> GetAllArtists()
        {
            return _MBoxContext.Artists;

        }
    }
}
