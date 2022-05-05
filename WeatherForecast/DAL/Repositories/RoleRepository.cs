using DAl.Repositories;
using DAL.data;
using DAL.IRepositories;
using DAL.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}