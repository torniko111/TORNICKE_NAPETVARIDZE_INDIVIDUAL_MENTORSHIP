using DAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);
        Task DeleteAsync(User user);
        Task<List<User>> ListAsync();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        //Task<double> GetWeatherApi(string city);

    }
}
