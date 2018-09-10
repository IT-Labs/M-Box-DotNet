using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelManager : IRecordLabelManager
    {
        private IRepository _repository;

        public RecordLabelManager(IRepository repository)
        {
            _repository = repository;
        }

        public IList<RecordLabel> GetAllRecordLabels()
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();
        }

        public IList<RecordLabel> GetNextRecordLabels(int toSkip, int toTake)
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)}", skip: toSkip, take: toTake).ToList();
        }

    }
}

