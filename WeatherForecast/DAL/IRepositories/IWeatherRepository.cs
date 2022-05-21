
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IWeatherRepository : IRepositoryBase<Weather>
    {
        Task<List<Weather>> GetByDateRange(DateTime from, DateTime to);
        Task<List<Weather>> AddRange(List<Weather> weathers);
    }
}
 