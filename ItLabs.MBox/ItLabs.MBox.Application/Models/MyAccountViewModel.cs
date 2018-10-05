using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application.Models
{
    public class MyAccountViewModel
    {
        public string Picture { get; set; }
        public string Name { get; set; }
        public string RecordLabelInfo { get; set; }
        public string ArtistBio { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
