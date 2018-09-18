using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Dtos
{
    public class AddMultipleArtistsDto
    {
        public IList<string> Errors;

        public IList<Artist> ArtistsToBeAdded;

        public AddMultipleArtistsDto()
        {
            Errors = new List<string>();
            ArtistsToBeAdded = new List<Artist>();
        }
    }
}
