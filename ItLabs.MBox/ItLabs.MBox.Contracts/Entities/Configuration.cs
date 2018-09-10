using ItLabs.MBox.Contracts.Enums;

namespace ItLabs.MBox.Contracts.Entities
{
    public class Configuration : Entity
    {
        public virtual ConfigurationKey Key { get; set; }

        public virtual string Value { get; set; }
    }
}
