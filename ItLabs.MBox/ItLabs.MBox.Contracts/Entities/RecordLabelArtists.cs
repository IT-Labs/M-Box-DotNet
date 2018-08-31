namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabelArtists : AuditableEntity
    {
        public virtual Artist Artist { get; set; }
        public virtual RecordLabel RecordLabel { get; set; }

    }
}
