using Microsoft.EntityFrameworkCore;
using MinimalChatApplicationAPI.Models;

namespace MinimalChatApplicationAPI.Data
{
    public class MinimalChatDbContext : DbContext
    {
        public MinimalChatDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
