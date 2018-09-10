using System;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        int CreatedBy { get; set; }
        DateTime DateCreated { get; set; }
        int? ModifiedBy { get; set; }
        DateTime DateModified { get; set; }
    }
}
