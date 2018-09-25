using ItLabs.MBox.Contracts;
using ItLabs.MBox.Contracts.Interfaces;
using System.Collections.Generic;

namespace ItLabs.MBox.Application.Models
{
    public class PagingModel<T> where T : IEntity
    {
        public PagingModel()
        {
            Skip = MBoxConstants.initialSkip;
            Take = MBoxConstants.initialTakeHomeLists;
            PagingList = new List<T>();
        }
        public IList<T> PagingList { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchQuery { get; set; }
    }
}
