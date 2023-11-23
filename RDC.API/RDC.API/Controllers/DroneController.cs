using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDC.API.Context;
using RDC.API.Mapping;
using RDC.API.Models.Drone;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RDC.API.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class DroneController : ControllerBase
        {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DroneController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DroneDTO>> GetDrone(int id)
        {
            var drone = await _context.Drone.FindAsync(id);

            if (drone is null)
            {
                return BadRequest(string.Format("Drone with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (user.CompanyId != drone.CompanyId)
            {
                return BadRequest(string.Format("This user can't access drones from another company"));
            }

            return Ok(drone);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DroneDTO>>> GetDrones()
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var drones = _context.Drone.Where(d => d.CompanyId == user.CompanyId).ToList();

            if (drones is null || drones.Count() == 0)
            {
                return NoContent();
            }

            return Ok(drones);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<DroneDTO>> CreateDrone([FromBody] DroneDTO createDroneDto)
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var drone = _mapper.Map<DroneModel>(createDroneDto);

            drone.CompanyId = user.CompanyId;

            await _context.Drone.AddAsync(drone);

            await _context.SaveChangesAsync();

            var createdCategory = await _context.Drone.FindAsync(drone.Id);

            if (createdCategory is null)
            {
                return BadRequest("Unable to create drone");
            }

            return Ok(createDroneDto);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}")]
        public async Task<ActionResult> ChangeDrone(int id, [FromBody] DroneDTO changeDroneDto)
        {
            var drone = await _context.Drone.FindAsync(id);

            if (drone is null)
            {
                return BadRequest(string.Format("Drone with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (user.CompanyId != drone.CompanyId)
            {
                return BadRequest(string.Format("This user can't edit drones from another company"));
            }

            DroneMappingExtensions.ProjectFrom(drone, changeDroneDto);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteDrone(int id)
        {
            var drone = await _context.Drone.FindAsync(id);

            if (drone is null)
            {
                return BadRequest(string.Format("Drone with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (user.CompanyId != drone.CompanyId)
            {
                return BadRequest(string.Format("This user can't delete drones from another company"));
            }

            _context.Drone.Remove(drone);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
