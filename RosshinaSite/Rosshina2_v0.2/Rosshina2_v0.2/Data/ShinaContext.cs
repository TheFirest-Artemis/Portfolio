using Microsoft.EntityFrameworkCore;
using Rosshina2_v0._2.Models;

namespace Rosshina2_v0._2.Data
{
    public class ShinaContext : DbContext
    {
        public DbSet<MaterialsModel> MaterialsModels { get; set; }

        public ShinaContext(DbContextOptions<ShinaContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=mysql94.1gb.ru;database=gb_kafeorder2;user=gb_kafeorder2;password=X3KZLMD-Da3Z");
        }

    }
}
