using BL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        public Task<Weather> AddAsync(Weather weather)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Weather weather)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Weather weather)
        {
            throw new NotImplementedException();
        }
    }
}
