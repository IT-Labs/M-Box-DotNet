using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class RecordLabelViewModel
    {
        public IList<RecordLabel> RecordLabels { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
