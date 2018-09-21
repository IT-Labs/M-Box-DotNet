using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class ConfigurationManager : BaseManager<Configuration>, IConfigurationManager
    {
        public ConfigurationManager(IRepository repository) : base(repository)
        {

        }
    }
}
