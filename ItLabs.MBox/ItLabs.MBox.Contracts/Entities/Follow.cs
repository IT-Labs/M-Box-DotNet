namespace ItLabs.MBox.Contracts.Entities
{
    public class Follow : AuditableEntity

    {
        public virtual Artist Artist { get; set; }

        public virtual ApplicationUser Follower { get; set; }
    }
}
