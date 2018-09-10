namespace ItLabs.MBox.Contracts.Entities
{
    public class RecordLabelArtist : Entity
    {
        public virtual int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual int RecordLabelId { get; set; }
        public virtual RecordLabel RecordLabel { get; set; }
    }
}
