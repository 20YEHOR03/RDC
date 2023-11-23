using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDC.API.Context;
using RDC.API.Models.Flight;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RDC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FlightController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDTO>> GetFlight(int id)
        {
            var flight = await _context.Flight.FindAsync(id);

            if (flight is null)
            {
                return BadRequest(string.Format("Flight with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var flightUser = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == flight.UserId.ToString());

            if (currentUser.CompanyId != flightUser.CompanyId)
            {
                return BadRequest(string.Format("This user can't access flights from another company"));
            }

            return Ok(flight);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> GetFlights()
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var flights = _context.Drone.Where(d => d.CompanyId == user.CompanyId).ToList();

            if (flights is null || flights.Count() == 0)
            {
                return NoContent();
            }

            return Ok(flights);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<FlightDTO>> CreateFlight([FromBody] FlightDTO flightDto)
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var flight = _mapper.Map<FlightModel>(flightDto);

            flight.UserId = user.Id;

            var drone = await _context.Drone.SingleOrDefaultAsync(d => d.Id == flight.DroneId);

            if (user.CompanyId != drone.CompanyId)
            {
                return BadRequest("Unable to use drone from another company");
            }

            await _context.Flight.AddAsync(flight);

            await _context.SaveChangesAsync();

            var createdFlight = await _context.Flight.FindAsync(flight.Id);

            if (createdFlight is null)
            {
                return BadRequest("Unable to create flight");
            }

            return Ok(flightDto);
        }



        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteFlight(int id)
        {
            var flight = await _context.Flight.FindAsync(id);

            if (flight is null)
            {
                return BadRequest(string.Format("Flight with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var flightUser = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == flight.UserId.ToString());

            if (currentUser.CompanyId != flightUser.CompanyId)
            {
                return BadRequest(string.Format("This user can't access flights from another company"));
            }

            _context.Flight.Remove(flight);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
