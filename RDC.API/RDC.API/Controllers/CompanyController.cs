using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;
using RDC.API.Models.Company;
using RDC.API.Models.User;
using RDC.API.Models.Drone;
using AutoMapper;
using RDC.API.Context;
using RDC.API.Mapping;

namespace RDC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public CompanyController(DataContext context, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _config = config;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> CompanyRegister(CompanyDTO companyDTO)
        {
            var company = _mapper.Map<CompanyModel>(companyDTO);
            _context.Company.Add(company);
            await _context.SaveChangesAsync();
            return Ok(companyDTO);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}")]
        public async Task<ActionResult> ChangeCompany(int id, [FromBody] CompanyDTO changeCompanyDto)
        {
            var company = await _context.Company.FindAsync(id);

            if (company is null)
            {
                return BadRequest(string.Format("Company with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (user.CompanyId != company.Id)
            {
                return BadRequest(string.Format("This user can't edit another company"));
            }

            CompanyMappingExtensions.ProjectFrom(company, changeCompanyDto);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeletCompany(int id)
        {
            var company = await _context.Company.FindAsync(id);

            if (company is null)
            {
                return BadRequest(string.Format("Company with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (user.CompanyId != company.Id)
            {
                return BadRequest(string.Format("This user can't delete another company"));
            }

            _context.Company.Remove(company);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
