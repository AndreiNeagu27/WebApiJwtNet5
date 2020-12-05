using System;

namespace WebApi.Models.Authorization
{
    public class AuthorizationRequest
    {
        public AuthorizationRequest()
        { }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
