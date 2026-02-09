using JWT_Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWT_Auth.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options): DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
