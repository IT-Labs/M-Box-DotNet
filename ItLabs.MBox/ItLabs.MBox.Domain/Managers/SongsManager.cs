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
        private IRepository<Follow> _followsRepository;

        public SongsManager(IRepository<Song> repository, IRepository<Follow> followsRepository)
        {
            _songsRepository = repository;
            _followsRepository = followsRepository;
        }

        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _songsRepository.GetAll()
                .Include(c => c.Artist.User)
                .OrderByDescending(x => x.ReleaseDate)
                .Take(number)
                .ToList();
        }

        public IList<Song> GetRecentlyAddedSongsOfMostPopularArtists(int number)
        {
            var mostFollowedArtist = _followsRepository.GetAll()
                .Include(x => x.Artist.User)
                .GroupBy(x => x.Artist)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key).FirstOrDefault();

            return _songsRepository.GetAll()
                .Include(x => x.Artist.User)
                .Where(x => x.Artist == mostFollowedArtist)
                .OrderByDescending(x => x.ReleaseDate)
                .Take(number).ToList();
                

        }
    }
}
