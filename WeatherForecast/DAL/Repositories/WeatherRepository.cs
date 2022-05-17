using DAl.Repositories;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class WeatherRepository : RepositoryBase<Weather>, IWeatherRepository
    {
        public WeatherRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Weather>> GetByDateRange(DateTime from, DateTime to)
        {
            return await _dbContext.Weathers.Where(x => x.CreatedOn >= from && x.CreatedOn <= to).ToListAsync();
        }
    }
}
