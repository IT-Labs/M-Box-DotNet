using ItLabs.MBox.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts.Dtos
{
    public class AddMultipleArtistsDto
    {
        public IList<string> Errors;

        public IList<ApplicationUser> UsersToBeAdded;

        public AddMultipleArtistsDto()
        {
            Errors = new List<string>();
            UsersToBeAdded = new List<ApplicationUser>();
        }
    }
}
