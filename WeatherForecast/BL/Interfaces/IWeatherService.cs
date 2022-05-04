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
        Task<List<User>> ListAsync();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(Weather weather);
    }
}
