using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IAuthService
    {
        Task<int> Register(User user, string password);
        Task<bool> UserExists(string email);
        Task<string> Login(string email, string password);
        Task<bool> ChangePassword(int userId, string newPassword);
        int GetUserId();
        string GetUserEmail();
        Task<User> GetUserByEmail(string email);
    }
}
