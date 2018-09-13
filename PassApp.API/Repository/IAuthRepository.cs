using System.Threading.Tasks;
using PassApp.API.Models;

namespace PassApp.API.Repository
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserExists(string userName);
        Task<bool> ChangeEntryCode(string userName, string oldPass, string newPass);
    }
}