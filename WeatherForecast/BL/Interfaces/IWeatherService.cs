using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IWeatherService
    {
        Task<Weather> AddAsync(Weather weather);
        Task DeleteAsync(Weather weather);
        Task<List<Weather>> ListAsync();
        Task<Weather> GetByIdAsync(int id);
        Task UpdateAsync(Weather weather);
        Task<List<Weather>> GetReportByDate(DateTime from, DateTime to);
    }
}
