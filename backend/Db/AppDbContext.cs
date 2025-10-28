using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Worker> Workers => Set<Worker>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
