namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabel : AuditableEntity
    {
        public virtual ApplicationUser UserId { get; set; }

        public string AboutInfo { get; set; }
    }
}
