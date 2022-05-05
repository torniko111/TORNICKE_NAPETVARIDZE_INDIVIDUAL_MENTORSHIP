using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IRepositories
{
    public interface IWeatherRepository : IRepositoryBase<Weather>
    {
    }
}
