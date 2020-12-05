using System;
using System.Text.Json.Serialization;

namespace WebApi.Models.Users
{
    public class User
    {
        public User()
        {
        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}
