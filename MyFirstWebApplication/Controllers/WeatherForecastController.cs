using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebApplication.Controllers
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

        [HttpPut("increase/{value}", Name = "IncreaseInt")]
        public ActionResult<int> IncreaseInt(int value)
        {
            int increasedValue = value + 1;
            return Ok(increasedValue);
        }

        [HttpPut("add", Name = "AddTwoNumbers")]
        public ActionResult<int> AddTwoNumbers([FromBody] AddNumbersRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            int sum = request.Number1 + request.Number2;
            return Ok(sum);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
    public class AddNumbersRequest
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
    }
}