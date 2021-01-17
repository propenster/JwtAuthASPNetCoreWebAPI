using JwtAuthenticationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationWebAPI.Services
{
    public interface IJwtAuthenticationHandler
    {

        UserModel AuthenticateUser(UserModel userInfo);
        string GenerateJwtTokens(UserModel userInfo);

    }
}
