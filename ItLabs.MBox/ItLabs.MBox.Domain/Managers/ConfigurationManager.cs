using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Domain.Managers
{
    public class ConfigurationManager : BaseManager<Configuration>, IConfigurationManager
    {
        public ILogger _logger { get; set; }
        public ConfigurationManager(IRepository repository, ILogger<Configuration> logger) : base(repository, logger)
        {

        }
    }
}
