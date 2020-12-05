using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models.Settings;
using Microsoft.Extensions.Options;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Helpers
{
    public class JwtHelper
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly AppSettings _appSettings;

        public JwtHelper(RequestDelegate requestDelegate, IOptions<AppSettings> appSettings)
        {
            _requestDelegate = requestDelegate;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUsersService userService)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                UserToContext(context, userService, token);

            await _requestDelegate(context);
        }

        private void UserToContext(HttpContext context, IUsersService userService, string token)
        {
            try
            {
                JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Key);

                jwtHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // setting ClockSkew to zero makes tokens expire on the spot, while leaving it to default will make them expire after 5 minutes
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                Guid userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "userid").Value);

                // if jwt validation is successful the user is attahced to the token
                context.Items["User"] = userService.GetById(userId);
            }
            catch
            {
                //if validation fails ... just do nothing
                //if the user was not attached to the context all requests from user will not have access to the secure routes
                //if you really want to do something here ... maybe you can log an unsuccessful login attempt
            }
        }
    }
}
