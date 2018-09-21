using ItLabs.MBox.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class PagingModel<T> where T : IEntity
    {
        public IList<T> PagingList { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchQuery { get; set; }
    }
}
