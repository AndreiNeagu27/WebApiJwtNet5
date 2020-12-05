using System;
using System.Text.Json.Serialization;
using WebApi.Models.Users;

namespace WebApi.Models.Authorization
{
    public class AuthorizationResponse
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }

        public AuthorizationResponse(User user, string token)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Password = user.Password;
            Token = token;
        }
    }
}
