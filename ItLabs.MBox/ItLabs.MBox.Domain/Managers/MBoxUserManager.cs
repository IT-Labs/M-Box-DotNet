using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItLabs.MBox.Domain.Managers
{
    public class MBoxUserManager : UserManager<ApplicationUser>
    {
        IRepository _repository;
        public MBoxUserManager(IRepository repository,IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _repository = repository;
        }

        public async Task<ApplicationUser> CreateUser(string name, string email, Role role, string password = null)
        {
            if (password == null)
                password = Guid.NewGuid().ToString();

            var user = new ApplicationUser { Name = name, UserName = email, Email = email };
            var result = await CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return null;
            }
            var roleResult = AddToRoleAsync(user, role.ToString()).Result;

            if (!roleResult.Succeeded)
            {
                return null;
            }

            return user;

        }
    }
}