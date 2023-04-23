using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Repositories
{
    public interface IAuthRepository : IGenericRepository<User>
    {
        Task<bool> UserExists(string email);
        Task<User> FindEmail(string email);
        Task<User> FindUserById(int id);
        Task<User> GetUserByEmail(string email);

    }
}
