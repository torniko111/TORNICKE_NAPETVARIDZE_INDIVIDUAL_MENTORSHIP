using BL.Interfaces;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            this.weatherRepository = weatherRepository;
        }


        public Task<Weather> AddAsync(Weather weather)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Weather weather)
        {
            throw new NotImplementedException();
        }

        public Task<List<Weather>> GetByDateRange(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<Weather> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task<List<Weather>> ListAsync()
        {
            return weatherRepository.ListAsync();
        }

        public Task UpdateAsync(Weather weather)
        {
            throw new NotImplementedException();
        }

    }
}
