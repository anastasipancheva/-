using System;
using System.Threading.Tasks;
using Back.Models;

namespace Back.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid id);
        Task AddAsync(User user);
    }
}
