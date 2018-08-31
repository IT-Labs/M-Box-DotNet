using ItLabs.MBox.Contracts.Entities;
using System.Linq;

namespace ItLabs.MBox.Contracts.Interfaces
{
    public interface IUsersRepository
    {
        ApplicationUser GetUserById(int id);
        ApplicationUser GetUserByEmail(string email);
    }
}
