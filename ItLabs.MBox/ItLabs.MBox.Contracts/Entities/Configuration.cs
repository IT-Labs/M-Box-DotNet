using ItLabs.MBox.Contracts.Interfaces;
using System;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Configuration : IAuditable
    {

        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }
  
        public int ModifiedBy { get; set; }
    }
}
