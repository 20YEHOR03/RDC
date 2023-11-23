using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDC.API.Models.Flight;
using System.Security.Claims;
using RDC.API.Models.SubscriptionPayment;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RDC.API.Context;

namespace RDC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SubscriptionPaymentController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionPaymentDTO>> GetSubscriptionPayment(int id)
        {
            var subscriptionPayment = await _context.SubscriptionPayment.FindAsync(id);

            if (subscriptionPayment is null)
            {
                return BadRequest(string.Format("Flight with id {0} is not found", id));
            }

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            if (currentUser.CompanyId != subscriptionPayment.CompanyId)
            {
                return BadRequest(string.Format("This user can't access subscriptions from another company"));
            }

            return Ok(subscriptionPayment);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPaymentDTO>>> GetSubscriptionPayments()
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            var subscriptionPayments = _context.SubscriptionPayment.Where(d => d.CompanyId == user.CompanyId).ToList();

            if (subscriptionPayments is null || subscriptionPayments.Count() == 0)
            {
                return NoContent();
            }

            return Ok(subscriptionPayments);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<SubscriptionPaymentModel>> PaySubscription(int subscriptionId)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.User.SingleOrDefaultAsync(u => u.Id.ToString() == currentUserId);

            SubscriptionPaymentModel lastPayment = _context.SubscriptionPayment.OrderByDescending(d => d.DurationDate).FirstOrDefault(d => d.CompanyId == user.CompanyId );

            var lastTime = DateTime.UtcNow.AddMonths(1);

            if (lastPayment != null && lastPayment.DurationDate.CompareTo(DateTime.UtcNow) > 0)
            {
                lastTime = lastPayment.DurationDate.AddMonths(1);
            }

            var subscriptionPayment = new SubscriptionPaymentModel()
            {
                DurationDate = lastTime,
                SubscriptionId = subscriptionId,
                CompanyId = user.CompanyId
            };

            await _context.SubscriptionPayment.AddAsync(subscriptionPayment);

            await _context.SaveChangesAsync();

            var createdSubscriptionPayment = await _context.SubscriptionPayment.FindAsync(subscriptionPayment.Id);

            if (createdSubscriptionPayment is null)
            {
                return BadRequest("Unable to pay");
            }

            return Ok(subscriptionPayment);
        }
    }
}
