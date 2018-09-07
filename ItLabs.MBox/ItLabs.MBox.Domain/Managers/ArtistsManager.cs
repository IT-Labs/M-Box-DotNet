using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ItLabs.MBox.Contracts.Data_Transfer_Objects;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistsManager : IArtistsManager
    {
        private IRepository<Artist> _artistsRepostiory;
        private IRepository<Follow> _followsRepository;
        private IRepository<RecordLabelArtist> _recordLabelArtistsRepository;

        public ArtistsManager(IRepository<Artist> artistsRepository, IRepository<Follow> followsRepository, IRepository<RecordLabelArtist> recordLabelArtistsRepository)
        {
            _artistsRepostiory = artistsRepository;
            _followsRepository = followsRepository;
            _recordLabelArtistsRepository = recordLabelArtistsRepository;
        }
        public IList<Artist> GetAllArtists()
        {
            return _artistsRepostiory.GetAll().Include(x => x.RecordLabelArtists).ThenInclude(x => x.Artist.User).ToList();
        }
        public IList<ArtistDto> GetMostFollowedArtists(int number)
        {
            var artistDtoListToReturn = new List<ArtistDto>();
            var mostFollowedArtists = _followsRepository.GetAll()
                .Include(x => x.Artist.User)
                .GroupBy(x => x.Artist)
                .OrderByDescending(x => x.Count())
                .Take(number)
                .Select(x => x.Key);
                //.ToList();
            foreach (var artist in mostFollowedArtists)
            {
                var dto = new ArtistDto();
                dto.Artist = artist;
                dto.Artist.User = _artistsRepostiory.GetAll().Include(x=>x.User).Where(x=>x == artist).FirstOrDefault().User;
                dto.RecordLabel = _recordLabelArtistsRepository.GetAll()
                    .Include(x => x.RecordLabel.User).Include(x => x.Artist.User)
                    .Where(x => x.Artist == artist).FirstOrDefault().RecordLabel;
                artistDtoListToReturn.Add(dto);
            }
            return artistDtoListToReturn;
        }
    }
}
