using Microsoft.EntityFrameworkCore;
using PassApp.API.Models;

namespace PassApp.API.Persistence
{
    public class PassDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        
        public PassDbContext(DbContextOptions<PassDbContext> options): base(options)
        {
        }
    }
}