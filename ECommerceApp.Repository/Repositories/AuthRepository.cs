using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Repository.Repositories
{
    public class AuthRepository : GenericRepository<User>,IAuthRepository
    {
        public AuthRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<bool> UserExists(string email)
        {
            return await _appDbContext.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<User> FindEmail(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<User> FindUserById(int id)
        {
            return await _appDbContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
