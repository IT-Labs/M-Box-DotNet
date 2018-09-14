using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using System.Collections.Generic;
using System.Linq;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelManager : BaseManager<RecordLabel> ,IRecordLabelManager
    {
        private readonly IRepository _repository;
        public RecordLabelManager(IRepository repository) : base(repository)
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
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}", skip: toSkip, take: toTake).ToList();
        }

    }
}

