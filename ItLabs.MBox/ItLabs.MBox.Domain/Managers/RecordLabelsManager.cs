using ItLabs.MBox.Contracts.Data_Transfer_Objects;
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
        private IRepository<RecordLabelArtists> _recordLabelArtistsRepository;

        public RecordLabelsManager(IRepository<RecordLabel> recordLabelReposotiry, IRepository<RecordLabelArtists> recordLabelArtistsReposotory)
        {
            _recordLabelRepository = recordLabelReposotiry;
            _recordLabelArtistsRepository = recordLabelArtistsReposotory;
        }

        public IList<RecordLabel> GetAllRecordLabels()
        {
            return _recordLabelRepository.GetAll().Include(x => x.User).ToList();
        }

        public IList<RecordLabelNumberOfArtistsDto> RecordLabelNumberOfArtists()
        {
            var listToReturn = new List<RecordLabelNumberOfArtistsDto>();
            var recordLabels = _recordLabelRepository.GetAll().Include(x => x.User);
            var recordLabelArtists = _recordLabelArtistsRepository.GetAll();


            foreach (var rl in recordLabels)
            {
                var rlNumArt = new RecordLabelNumberOfArtistsDto();
                //if ()
                //{
                    rlNumArt.RecordLabel = rl;
                    rlNumArt.NumberOfArtists = 12;
                //}
                //else
                /*{
                    rlNumArt.RecordLabel = rl;
                    rlNumArt.NumberOfArtists = 0;
                }*/
                
                listToReturn.Add(rlNumArt);
            }

            return listToReturn;
        }
    }
}
