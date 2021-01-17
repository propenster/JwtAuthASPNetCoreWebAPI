using JwtAuthenticationWebAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationWebAPI.Services
{
    public class JwtAuthenticationHandler : IJwtAuthenticationHandler
    {
        IConfiguration _config;

        public JwtAuthenticationHandler(IConfiguration config)
        {
            _config = config;
        }
        public UserModel AuthenticateUser(UserModel userInfo)
        {
            UserModel login = null;

            //validate now
            if(userInfo.Username == _config["UserAuthenticationDetails:Username"] && userInfo.Password == _config["UserAuthenticationDetails:Password"])
            {
                login = new UserModel
                {
                    Username = _config["UserAuthenticationDetails:Username"],
                    EmailAddress = _config["UserAuthenticationDetails:EmailAddress"],
                    Password = _config["UserAuthenticationDetails:Password"]
                };
            }
            return login;
        }

        public string GenerateJwtTokens(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //define claims...
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //define token and write
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: credentials
                );
            //fetch encoded Token
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;


        }
    }
}
