using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthenticationWebAPI.Models;
using JwtAuthenticationWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JwtAuthenticationWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IJwtAuthenticationHandler _jwtHandler;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IJwtAuthenticationHandler jwtHandler)
        {
            _logger = logger;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = _jwtHandler.AuthenticateUser(login);
            if(user != null)
            {
                var tokenString = _jwtHandler.GenerateJwtTokens(user);
                response = Ok(new { user, tokenString }); 
                
            }

            return response;

        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
