namespace ItLabs.MBox.Contracts.Entities
{
    public class Follow : AuditableEntity

    {
        public virtual int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual int FollowerId { get; set; }
        public virtual ApplicationUser Follower { get; set; }
    }
}
