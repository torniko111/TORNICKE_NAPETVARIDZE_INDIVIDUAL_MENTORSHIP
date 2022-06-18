using DAL.data;
using DAL.GenericRepository;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.TypeRepository
{
    public class WeatherRepository : GenericRepository<Weather>, IWeatherRepository
    {
        public WeatherRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Weather>> AddRange(List<Weather> weathers)
        {
            await context.Weathers.AddRangeAsync(weathers);
            context.SaveChanges();
            return weathers;
        }

        public async Task<List<Weather>> GetByDateRange(DateTime from, DateTime to, string city)
        {
            return await context.Weathers.Where(x => x.CreatedOn >= from && x.CreatedOn <= to && x.CityName == city).ToListAsync();
        }
    }
}
