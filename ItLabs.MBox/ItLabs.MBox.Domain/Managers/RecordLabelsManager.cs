using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelsManager : IRecordLabelsManager
    {
        private IRepository<RecordLabel> _recordLabelRepository;

        public RecordLabelsManager(IRepository<RecordLabel> recordLabelReposotiry)
        {
            _recordLabelRepository = recordLabelReposotiry;
        }

        public IList<RecordLabel> GetAllRecordLabels()
        {
            return _recordLabelRepository.GetAll().Include(x => x.User).ToList();
        }

        public IList<RecordLabel> GetNextRecordLabels(int skip, int take)
        {
            return _recordLabelRepository.GetAll().Include(x => x.User).Skip(skip).Take(take).ToList();
        }
    }
}
