using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Configuration : IAuditable
    {

        public int ConfigurationId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }
  
        public int ModifiedBy { get; set; }
    }
}
