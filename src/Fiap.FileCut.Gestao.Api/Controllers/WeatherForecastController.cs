using Fiap.FileCut.Core.Interfaces.Services;
using Fiap.FileCut.Core.Objects;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Fiap.FileCut.Gestao.Api.Controllers
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

        private readonly INotifyService _notifyService;

        public WeatherForecastController(INotifyService notifyService,
            ILogger<WeatherForecastController> logger)
        {
            _notifyService = notifyService;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var message = new FileCutMailMessage("test2@test.com")
            {
                Subject = "Test",
                Body = "Test"
            };

            _notifyService.Notify(new NotifyContext<FileCutMailMessage>(message, Guid.NewGuid()));

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
