
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.data
{
    public class ApplicationDbContext : DbContext
    {
       
        public DbSet<Weather> Weathers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
