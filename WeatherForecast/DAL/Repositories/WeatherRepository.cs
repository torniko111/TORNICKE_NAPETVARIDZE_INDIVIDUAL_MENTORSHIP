using DAl.Repositories;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class WeatherRepository : RepositoryBase<Weather>, IWeatherRepository
    {
        public WeatherRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
