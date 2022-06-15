using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IWeatherRepository Weather { get;}
        int Save();
    }
}