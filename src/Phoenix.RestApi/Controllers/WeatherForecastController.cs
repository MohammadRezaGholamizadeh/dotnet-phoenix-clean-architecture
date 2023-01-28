namespace Phoenix.RestApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tests")]
    [ApiVersion("1.0")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            throw new Exception("Yes");
        }
    }
}