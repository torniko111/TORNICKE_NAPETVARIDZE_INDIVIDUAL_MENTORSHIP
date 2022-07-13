using BL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IWeatherService
    {
        Task DeleteAsync(Weather user);
        Task<List<Weather>> ListAsync();
        Task<Weather> GetByIdAsync(int id);
        Task UpdateAsync(Weather user);
        Task<double> AddWeather(string city);
        Task GetCurrentWeatherByCity(string city);
        Task GetCurrentWeatherByCitiesSameTime(string[] city);
        Task<List<Weather>> Getreport(DateTime from, DateTime to, string city);
        Task<Dictionary<double, string>> GetWeatherForecast(string city, int days);
        Task<string> AverageStatistics(string city, string period);
        void Subcribe(string name);
        void UnSubcribe(string name);
    }
}
