
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.data
{
    public class ApplicationDbContext : DbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Weather> Weathers { get; set; }
    }
}
