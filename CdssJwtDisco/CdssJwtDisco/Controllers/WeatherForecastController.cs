using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CdssJwtDisco.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Post")]
        public void Post()
        {
            var tokenHeader = Request.Headers["token"];
            var handler = new JwtSecurityTokenHandler();

            // what is difference between ReadJwtToken and ReadToken
            var token = handler.ReadJwtToken(tokenHeader);

            var sig = token.Claims.Single(c => c.Type.Equals("sig")).Value;

            var clientName = Request.Headers["client"];
            var resourcePath = "/institution/3181/customer/11212";

            using var ms = new MemoryStream();
            Request.Body.CopyToAsync(ms);
            var requestBody = Encoding.UTF8.GetString(ms.ToArray());
            var signed_payload = $"{resourcePath}.{requestBody}";

            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(clientName));
            var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(signed_payload));

            var sigStr = BitConverter.ToString(hash)
                                     .Replace("-", string.Empty);
            if (sigStr.Equals(sig))
            {
                _logger.LogInformation("valid");
            }
            else
            {
                _logger.LogInformation("tampered");
            }
        }

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
