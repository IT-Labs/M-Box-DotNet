using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using ItLabs.MBox.Domain.Managers;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItLabs.MBox.Application
{
    public class StructuremapRegistry : Registry
    {
        public StructuremapRegistry()
        {
            For<IArtistsManager>().LifecycleIs(Lifecycles.Container).Use<ArtistsManager>();
            For<IEmailManager>().LifecycleIs(Lifecycles.Container).Use<EmailsManager>();
            For<ISongsManager>().LifecycleIs(Lifecycles.Container).Use<SongsManager>();
            For(typeof(IRepository<>)).LifecycleIs(Lifecycles.Container).Use(typeof(Repository<>));
        }
    }
}
