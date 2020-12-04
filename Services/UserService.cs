using JwtService.Entities;
using JwtService.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtService.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        public AppSettings _appSettings;

        public List<User> users = new List<User>
        {
            new User
            {
                id = 1,
                FirstName = "Test",
                LastName = "User",
                Password = "test"
            }
        };

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {

            //User Authentication
            var user = users.SingleOrDefault(x => x.FirstName == username && x.Password == password);

            if(user == null)
            {
                return null;
            }

            //authentication successful so generate JWTToken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            //Remove password in response
            user.Password = null;
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return users.Select(x =>
            {
                x.Password = null;
                return x;
            });
        }
    }
}
