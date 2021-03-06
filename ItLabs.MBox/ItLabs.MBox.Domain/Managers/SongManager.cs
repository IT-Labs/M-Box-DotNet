﻿using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class SongManager : BaseManager<Song>, ISongManager
    {
        private IRepository _repository;

        public SongManager(IRepository repository, ILogger<Song> logger):base(repository,logger)
        {
            _repository = repository;
        }
        public IList<Song> GetRecentlyAddedSongs(int number)
        {
            return _repository.GetAll<Song>(includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.DateCreated), take: number).ToList();

        }

        public IList<Song> GetMostPopularArtistSongs(int number)
        {
            var mostFollowedArtist = _repository.GetAll<Artist>(orderBy: x => x.OrderByDescending(y => y.Follows.Count)).FirstOrDefault();
            if (mostFollowedArtist == null) { return new List<Song>(); }
            return _repository.Get<Song>(filter: x => x.Artist == mostFollowedArtist, includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)}", orderBy: x => x.OrderByDescending(y => y.DateCreated), take: number)
                .ToList();


        }

        public IList<Song> GetArtistSongs(int artistId, int toSkip, int toTake, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
                searchValue = string.Empty;
            return _repository.Get<Song>(
                filter: x => x.ArtistId == artistId && (x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.AlbumName.ToUpper().Contains(searchValue.ToUpper()) || x.Genre.ToString().ToUpper().Contains(searchValue.ToUpper())),
                includeProperties: $"{nameof(Song.Artist)}",
                skip: toSkip,
                take: toTake
                ).ToList(); ;
        }
    }
}
