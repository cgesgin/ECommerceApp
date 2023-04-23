using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class AuthService : GenericService<User>, IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration,IAuthRepository authRepository, IGenericRepository<User> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            var user = await _authRepository.FindUserById(userId);
            
            if (user == null)
            {
                return false;
            }
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _authRepository.GetUserByEmail(email);
        }

        public string GetUserEmail()
        {
            throw new NotImplementedException();
        }

        public int GetUserId()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _authRepository.FindEmail(email);
            if (user == null)
            {
                return null;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return "Fail Password";
            }
            else 
            {
                return CreateToken(user);
            }
            
        }

        public async Task<int> Register(User user, string password)
        {
            if (await _authRepository.UserExists(user.Email))
            {
                return 0;
            }

            CreatePasswordHash(password,out byte[] passwordHash,out byte[] passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _authRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();
            return user.Id;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _authRepository.UserExists(email))
            {
                return true;
            }
            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
               // new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes("Top Secret Token Key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
