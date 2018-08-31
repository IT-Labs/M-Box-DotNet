namespace ItLabs.MBox.Contracts.Entities
{
    public class Configuration : AuditableEntity
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
