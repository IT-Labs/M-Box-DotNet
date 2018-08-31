using ItLabs.MBox.Contracts.Entities;
using System.Linq;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private MBoxDbContext _mboxContext;

        public UsersRepository(MBoxDbContext context)
        {
            _mboxContext = context;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return _mboxContext.ApplicationUsers.FirstOrDefault(x => x.Email == email);
        }

        public ApplicationUser GetUserById(int id)
        {
            return _mboxContext.ApplicationUsers.FirstOrDefault(x => x.Id == id);
        }
    }
}
