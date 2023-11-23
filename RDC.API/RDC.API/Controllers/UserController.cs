using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RDC.API.Context;
using RDC.API.Mapping;
using RDC.API.Models.Flight;
using RDC.API.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RDC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserController(DataContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("Update")]
        public async Task<ActionResult> ChangeUser([FromBody] UserDTO userDto)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == userId);

            UserMappingExtensions.ProjectFrom(user, userDto);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("UpdatePassword")]
        public async Task<ActionResult> ChangePassword([Required] string oldPassword, [Required] string newPassword)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == userId);

            var oldPasswordHash = HashPassword(oldPassword);

            if(user.Password != oldPasswordHash)
            {
                return BadRequest("Old password is not correct");
            }

            var newPasswordHash = HashPassword(newPassword);
            user.Password = newPasswordHash;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<ActionResult<List<UserRegister>>> UserRegister(UserRegister userLogin)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == userLogin.Email);

            if (user != null)
            {
                return BadRequest("User already exists");

            }

            userLogin.Password = HashPassword(userLogin.Password);
            _context.User.Add(_mapper.Map<UserModel>(userLogin));
            await _context.SaveChangesAsync();

            return Ok(userLogin);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Login")]
        public async Task<ActionResult<List<UserLogin>>> Login([FromBody] UserLogin userLogin)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == userLogin.Email);

            var passwordHash = HashPassword(userLogin.Password);

            if (user == null)
            {
                return NotFound("User not found");

            }

            if (user.Password != passwordHash)
            {
                return BadRequest("User's password is not correct");
            }

            var token = GenerateToken(user);
            return Ok(token);
        }

        private string HashPassword(string password)
        {
            var sha256 = SHA256.Create();

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password)); 

            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hash;
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var role = user.IsAdmin ? "Admin" : "Worker";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
