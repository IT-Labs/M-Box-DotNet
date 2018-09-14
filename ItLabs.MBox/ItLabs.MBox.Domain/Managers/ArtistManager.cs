using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;
using ItLabs.MBox.Data;

namespace ItLabs.MBox.Domain.Managers
{
    public class ArtistManager : BaseManager<Artist>, IArtistManager
    {
        private IRepository _repository;

        public ArtistManager(IRepository repository):base(repository)
        {
            _repository = repository;
        }

        public IList<Artist> GetAllUserArtists()
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}").ToList();
        }

        public IList<Artist> GetNextArtists(int toSkip, int toTake)
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}", skip: toSkip, take: toTake).ToList();
        }

        public IList<Artist> GetAllArtists()
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.RecordLabelArtists)},{nameof(Artist)},{nameof(Artist.User)}").ToList();
        }
        public IList<Artist> GetMostFollowedArtists(int number)
        {
            return _repository.GetAll<Artist>(
                includeProperties: $"{nameof(Artist.User)}," +
                                   $"{nameof(Artist.Follows)}," +
                                   $"{nameof(Artist.RecordLabelArtists)}.{nameof(RecordLabel)}.{nameof(RecordLabel.User)}",
                                   orderBy: x=> x.OrderByDescending(y => y.Follows.Count),
                                   take: number).ToList();
        }
    }
}
