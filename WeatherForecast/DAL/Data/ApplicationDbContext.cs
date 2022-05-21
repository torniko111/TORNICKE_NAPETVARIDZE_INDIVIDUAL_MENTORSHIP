
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.data
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = WeatherDb; Trusted_Connection = True; MultipleActiveResultSets = true");
            base.OnConfiguring (optionsBuilder);
        }
        public DbSet<Weather> Weathers { get; set; }
    }
}
