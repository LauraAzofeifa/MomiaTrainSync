using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DbInitializer.Seed(modelBuilder);
        }
    }
}