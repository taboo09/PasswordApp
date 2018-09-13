using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using PassApp.API.Persistence;
using System.Security.Cryptography;

namespace PassApp.API.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly PassDbContext _context;
        private readonly IPasswordHash _passHash;
        private readonly IEncryptPassword _encryptPass;
        public AppRepository(PassDbContext context, 
            IPasswordHash passHash,
            IEncryptPassword encryptPass)
        {
            _context = context;
            _passHash = passHash;
            _encryptPass = encryptPass;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<bool> DeleteUser(int id, string entryCode)
        {
            var users = await _context.Users.ToListAsync();
            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null) return false;

            if (!_passHash.VerifyPassword(entryCode, user.EntryCodeHash, user.EntryCodeSalt))
                return false;

            _context.Remove(user);

            return true;
        }

        public async Task<bool> PasswordExists(string name, int id)
        {
            return await _context.Passwords.AnyAsync(u => u.UserId == id && u.Name == name);
        }

        public async Task AddPassword(Password password, byte[] userKey)
        { 
            var key = Convert.ToBase64String(userKey);
    
            password.PasswordEncrypt = _encryptPass.EncryptString(password.PasswordEncrypt, key);

            await _context.Passwords.AddAsync(password);
        }

        public async Task<string> ViewPassword(int userId, int passId)
        {
            var password = await _context.Passwords.Include(x => x.User).FirstOrDefaultAsync(u => u.UserId == userId && u.Id == passId);
            var passwordString = _encryptPass.DecryptString(password.PasswordEncrypt, Convert.ToBase64String(password.User.UserKey));

            return passwordString;
        }

        public async Task<IEnumerable<Password>> GetPasswords(int userId)
        {
            var passwords = await _context.Passwords.Where(x => x.UserId == userId).ToListAsync();

            return passwords;
        }

        public async Task<bool> UpdatePassword(Password password)
        {
            var pass = await _context.Passwords.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == password.Id);

            if (pass == null) return false;

            var key = Convert.ToBase64String(pass.User.UserKey);

            if(pass.PasswordEncrypt != password.PasswordEncrypt) 
                password.PasswordEncrypt = _encryptPass.EncryptString(password.PasswordEncrypt, key);

            _context.Entry(pass).CurrentValues.SetValues(password);

            return true;
        }

        public async Task<bool> DeletePassword(int id)
        {
            var pass = await _context.Passwords.FirstOrDefaultAsync(x => x.Id == id);

            if (pass == null) return false;

            _context.Passwords.Remove(pass);

            return true;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}