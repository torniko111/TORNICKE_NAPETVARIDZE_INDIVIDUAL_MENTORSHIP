
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.data
{
    public class ApplicationDbContext : DbContext
    {
       
        public DbSet<Weather> Weathers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WeatherDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
