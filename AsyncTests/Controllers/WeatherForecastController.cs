using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace AsyncTests.Controllers
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
        private readonly WeatherService _ws;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService ws)
        {
            _logger = logger;
            _ws = ws;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            
            Console.WriteLine("");
            Console.WriteLine("REQUEST Thread: " + Thread.CurrentThread.ManagedThreadId);
            _ws.GetWeatherForCitiesThrottled(Cities.CityNames);

            Console.WriteLine("REQUEST POST CITYNAMES Thread: " + Thread.CurrentThread.ManagedThreadId);

            var rng = new Random();
            Console.WriteLine("RETURNING RESPONSE Thread: " + Thread.CurrentThread.ManagedThreadId);
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
