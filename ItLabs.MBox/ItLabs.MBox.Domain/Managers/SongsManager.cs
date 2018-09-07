using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class SongsManager : ISongsManager
    {
        private IRepository<Song> _songsRepository;
        private IRepository<Artist> _artistRepository;

        public SongsManager(IRepository<Song> repository, IRepository<Artist> artistRepository)
        {
            _songsRepository = repository;
            _artistRepository = artistRepository;
        }
        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _songsRepository.GetAll()
                .Include(c => c.Artist.User)
                .OrderByDescending(x => x.ReleaseDate)
                .Take(number)
                .ToList();
        }

        public IList<Song> GetRecentlyAddedSongsOfMostPopularArtist(int number)
        {
            var mostFollowedArtist = _artistRepository.GetAll().OrderByDescending(x => x.Follows.Count).FirstOrDefault();

            return _songsRepository.GetAll()
                .Include(x => x.Artist.User)
                .Where(x => x.Artist == mostFollowedArtist)
                .OrderByDescending(x => x.ReleaseDate)
                .Take(number).ToList();
                

        }
    }
}
