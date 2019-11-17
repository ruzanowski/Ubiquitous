using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.IdentityService.Domain.Domain;
using U.IdentityService.Persistance.Contexts;

namespace U.IdentityService.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _identityContext;

        public UserRepository(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<User> GetAsync(Guid id)
            => await _identityContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));

        public async Task<User> GetAsync(string email)
            => await _identityContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());

        public async Task AddAndSaveAsync(User user)
        {
            await _identityContext.Users.AddAsync(user);
            await _identityContext.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(User user)
        {
            await Task.CompletedTask;
            _identityContext.Users.Update(user);
            await _identityContext.SaveChangesAsync();

        }
    }
}