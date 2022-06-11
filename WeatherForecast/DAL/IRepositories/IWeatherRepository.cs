
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IWeatherRepository : IGenericRepository<Weather>
    {
        Task<List<Weather>> GetByDateRange(DateTime from, DateTime to, string city);
        Task<List<Weather>> AddRange(List<Weather> weathers);
    }
}
 