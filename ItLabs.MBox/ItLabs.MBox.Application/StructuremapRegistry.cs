using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using ItLabs.MBox.Domain.Managers;
using StructureMap;
using StructureMap.Pipeline;

namespace ItLabs.MBox.Application
{
    public class StructuremapRegistry : Registry
    {
        public StructuremapRegistry()
        {
            For<IRecordLabelManager>().LifecycleIs(Lifecycles.Container).Use<RecordLabelManager>();
            For<IArtistManager>().LifecycleIs(Lifecycles.Container).Use<ArtistManager>();
            For<IEmailsManager>().LifecycleIs(Lifecycles.Container).Use<EmailManager>();
            For<ISongManager>().LifecycleIs(Lifecycles.Container).Use<SongManager>();
            For<IS3Manager>().LifecycleIs(Lifecycles.Container).Use<S3Manager>();
            For<IConfigurationManager>().LifecycleIs(Lifecycles.Container).Use<ConfigurationManager>();
            For<ISearchManager>().LifecycleIs(Lifecycles.Container).Use<SearchManager>();

            For(typeof(IRepository)).LifecycleIs(Lifecycles.Container).Use(typeof(Repository<MBoxDbContext>));
            For(typeof(IReadOnlyRepository)).LifecycleIs(Lifecycles.Container).Use(typeof(ReadOnlyRepository<MBoxDbContext>));
        }
    }
}
