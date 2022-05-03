using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IRoleService
    {
        Task<Role> AddAsync(Role role);
        Task DeleteAsync(Role role);
        Task<List<Role>> ListAsync();
        Task<Role> GetByIdAsync(int id);
        Task UpdateAsync(Role role);
    }
}