using System;
using System.Threading.Tasks;
using U.IdentityService.Domain.Domain;

namespace U.IdentityService.Persistance.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAndSaveAsync(User user);
        Task UpdateAndSaveAsync(User user);
    }
}