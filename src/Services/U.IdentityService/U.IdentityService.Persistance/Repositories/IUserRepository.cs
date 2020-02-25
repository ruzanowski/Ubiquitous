using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using U.IdentityService.Domain.Models;

namespace U.IdentityService.Persistance.Repositories
{
    public interface IUserRepository
    {
        Task<IList<User>> GetUsersAsync();
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAndSaveAsync(User user);
        Task UpdateAndSaveAsync(User user);
    }
}