using Microsoft.EntityFrameworkCore;

namespace GameSIteV01.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<GameSIteV01.Models.UserModel> Users { get; set; }
    }
}
