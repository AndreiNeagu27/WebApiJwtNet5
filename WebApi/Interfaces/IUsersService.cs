using System;
using System.Collections.Generic;
using WebApi.Models.Authorization;
using WebApi.Models.Users;

namespace WebApi.Interfaces
{
    public interface IUsersService
    {
        AuthorizationResponse Authorize(AuthorizationRequest authorizationRequest);
        User GetUser(Guid userId);
        IEnumerable<User> GetAllUsers();
        User InsertUser(User user);
        List<User> InsertUsersBulk(List<User> users);
        User UpdateUser(User user);
        List<User> UpdateUsersBulk(List<User> users);
        string GenerateJwtToken(User user);
    }
}
