using DAl.Repositories;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
