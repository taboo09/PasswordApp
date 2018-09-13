using System.Collections.Generic;
using System.Threading.Tasks;
using PassApp.API.Models;

namespace PassApp.API.Repository
{
    public interface IAppRepository
    {
        Task<User> GetUser(int id);
        Task<bool> DeleteUser(int id, string entryCode);
        Task<bool> SaveAll();
        Task<bool> PasswordExists(string name, int id);
        Task<IEnumerable<Password>> GetPasswords(int userId);
        Task AddPassword(Password password, byte[] key);
        Task<string> ViewPassword(int userId, int passId);
        Task<bool> UpdatePassword(Password password);
        Task<bool> DeletePassword(int id);
    }
}