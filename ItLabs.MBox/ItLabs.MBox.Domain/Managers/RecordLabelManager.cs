using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelManager : BaseManager<RecordLabel> ,IRecordLabelManager
    {
        private readonly IRepository _repository;
        private readonly IEmailsManager _emailsManager;
        public RecordLabelManager(IRepository repository, IEmailsManager emailsManager) : base(repository)
        {
            _repository = repository;
            _emailsManager = emailsManager;
        }

        public IList<RecordLabel> GetAllRecordLabels()
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();
        }

        public IList<RecordLabel> GetNextRecordLabels(int toSkip, int toTake)
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}", skip: toSkip, take: toTake).ToList();
        }
        public void DeleteRecordLabel(ApplicationUser user)
        {
            var recordLabelArtists = _repository.Get<RecordLabelArtist>(filter: x => x.RecordLabel.User == user, includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)},{nameof(RecordLabel)}");
            var artists = recordLabelArtists.Select(x=>x.Artist);
            foreach(var artist in artists)
            {
                artist.IsDeleted = true;
                _repository.Update<Artist>(artist,user.Id);
                _emailsManager.SendMail(EmailTemplateType.DeletedArtist, artist.User.Email,"");
            }
            foreach(var rla in recordLabelArtists)
            {
                _repository.Delete(rla);
            }
            _emailsManager.SendMail(EmailTemplateType.DeletedRecordLabel, user.Email, "");
            _repository.Delete<RecordLabel>(user.Id);
            _repository.Delete(user);
            _repository.Save();
        }

        public IList<RecordLabel> GetSearchedRecordLabels(string searchValue, int toSkip, int toTake)
        {
            return _repository.Get<RecordLabel>(
                filter: x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()),
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}",
                skip: toSkip, take: toTake
                ).ToList();
        }
    }
}