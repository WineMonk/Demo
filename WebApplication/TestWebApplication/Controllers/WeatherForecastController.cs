using Microsoft.AspNetCore.Mvc;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            foreach (var item in _configuration.AsEnumerable())
            {

            }
            //��ȡ����ʱ��IP��Port(�˿�)
            string json = "NginxTest��" + _configuration["ip"] + ":" + _configuration["port"];
            await Task.Delay(1000);
            return json;
        }
    }
}