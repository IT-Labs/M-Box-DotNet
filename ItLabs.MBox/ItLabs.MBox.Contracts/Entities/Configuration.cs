namespace ItLabs.MBox.Contracts.Entities
{
    public class Configuration : AuditableEntity
    {
        public virtual string Key { get; set; }

        public virtual string Value { get; set; }
    }
}
