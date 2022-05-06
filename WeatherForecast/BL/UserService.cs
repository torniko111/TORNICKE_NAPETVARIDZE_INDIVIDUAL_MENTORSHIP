
using BL.Interfaces;
using DAL.IRepositories;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public static CommentContext comment;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public Task DeleteAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public  async Task<double> GetWeatherApi(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length == 1)
            {
                throw new ArgumentNullException(nameof(city));
            }

            //needs to be moved to appsettings
            string apikey = "1e2f66e8ba55167f95b01dd4c7364021";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org");

            var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apikey}");
            var stringResult = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<dynamic>(stringResult);

            var tmpdegreesc = Math.Round(((float)obj.main.temp - 272.15), 2);


            if (tmpdegreesc > 16)
            {
                comment = new CommentContext(new FreshStrategy());
            }
            else
            {
                comment = new CommentContext(new WarmlyStrategy());
            }
            Console.WriteLine(comment.Comment());

            return tmpdegreesc;
        }

        public Task<List<User>> ListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}