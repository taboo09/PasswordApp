using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PassApp.API.Models;
using PassApp.API.Persistence;

namespace PassApp.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PassDbContext _context;
        private readonly IPasswordHash _passHash;
        private readonly IEncryptPassword _encryptPassword;

        public AuthRepository(PassDbContext context, 
            IPasswordHash passHash,
            IEncryptPassword encryptPassword)
        {
            _context = context;
            _passHash = passHash;
            _encryptPassword = encryptPassword;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            
            _passHash.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.EntryCodeHash = passwordHash;
            user.EntryCodeSalt = passwordSalt;

            user.UserKey = _encryptPassword.GenerateKey();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return null;

            if (!_passHash.VerifyPassword(password, user.EntryCodeHash, user.EntryCodeSalt))
                return null;

            return user;
        } 

        public async Task<bool> UserExists(string userName)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == userName))
                return true;
            
            return false;
        }

        public async Task<bool> ChangeEntryCode(string userName, string oldPass, string newPass)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return false;

            if (!_passHash.VerifyPassword(oldPass, user.EntryCodeHash, user.EntryCodeSalt))
                return false;

            byte[] passwordHash, passwordSalt;
            
            _passHash.CreatePasswordHash(newPass, out passwordHash, out passwordSalt);
            user.EntryCodeHash = passwordHash;
            user.EntryCodeSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return true;
        }   
    }
}