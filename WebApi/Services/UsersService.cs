using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Interfaces;
using WebApi.Models.Authorization;
using WebApi.Models.Settings;
using WebApi.Models.Users;

namespace WebApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppSettings _appSettings;
        // for the example's sake I hardcoded a user ... you probably want to store them in a database ... yeah ... definetly do that ;)
        private readonly List<User> _users = new List<User> { new User { UserId = Guid.NewGuid(), UserName = "SomeUserName", Password = "SomePassword" } };

        public UsersService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthorizationResponse Authorize(AuthorizationRequest authorizationRequest)
        {
            //Identify user by username and password
            User user = _users.SingleOrDefault(x => x.UserName == authorizationRequest.UserName && x.Password == authorizationRequest.Password);

            // if user was not found, just return null
            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            string token = GenerateJwtToken(user);

            return new AuthorizationResponse(user, token);
        }

        public User GetUser(Guid userId)
        {
            User user = _users.FirstOrDefault(x => x.UserId == userId);

            //make sure that you have a result
            if(user != null && user.UserId != Guid.Empty)
            {
                return user;
            }
            else
            {
                return user; // but mention that no user was found
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            if(_users != null && _users.Count() > 0)
            {
                return _users;
            }
            else
            {
                return _users; // make sure to mention that no results where found
            }
        }

        /*
         BEGIN - Repeat process for the inserts and updates
         */

        public User InsertUser(User user)
        {
            return _users.FirstOrDefault();
        }

        public List<User> InsertUsersBulk(List<User> users)
        {
            return _users;
        }

        public User UpdateUser(User user)
        {
            return _users.FirstOrDefault();
        }

        public List<User> UpdateUsersBulk(List<User> users)
        {
            return _users;
        }

        /*
         END - Repeat process for the inserts and updates
         */

        public string GenerateJwtToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userid", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7), //generate token that is valid for 7 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
