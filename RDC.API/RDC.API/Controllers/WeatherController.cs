using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RDC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<string>> GetWeather([Required] double temperature, [Required] double humidity)
        {
            var maxTemp = 40d;
            var maxHumidity = 85d;
            if (temperature > maxTemp || humidity > maxHumidity)
            {
                return BadRequest("Flight is forbidden");
            }
            return Ok("Flight is allowed");
        }
    }
}
