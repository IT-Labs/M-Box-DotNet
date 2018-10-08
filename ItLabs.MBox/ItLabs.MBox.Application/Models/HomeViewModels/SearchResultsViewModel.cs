using ItLabs.MBox.Contracts.Data_Structures;
using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models.HomeViewModels
{
    public class SearchResultsViewModel
    {
        public PriorityQueue<object> Results;
        public string  SearchValue { get; set; }
        public SearchType SearchType { get; set; }

        public SearchResultsViewModel()
        {
            Results = new PriorityQueue<object>();
            SearchValue = string.Empty;
            SearchType = SearchType.MostRelevant;
        }
    }
}
