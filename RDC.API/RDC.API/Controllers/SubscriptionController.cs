//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using RDC.API.Context;
//using RDC.API.Mapping;
//using RDC.API.Models.Subscription;

//namespace RDC.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SubscriptionController : ControllerBase
//    {
//        private readonly DataContext _context;
//        private readonly IMapper _mapper;

//        public SubscriptionController(DataContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [HttpGet("{id}")]
//        public async Task<ActionResult<SubscriptionDTO>> GetSubscription(int id)
//        {
//            var subscription = await _context.Subscription.FindAsync(id);

//            if (subscription is null)
//            {
//                return BadRequest(string.Format("Subscription with id {0} is not found", id));
//            }

//            return Ok(subscription);
//        }

//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<SubscriptionDTO>>> GetSubscriptions()
//        {
//            var subscriptions = _context.Drone.ToList();

//            if (subscriptions is null || subscriptions.Count() == 0)
//            {
//                return NoContent();
//            }

//            return Ok(subscriptions);
//        }

//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [HttpPost]
//        public async Task<ActionResult<SubscriptionDTO>> CreateSubscription([FromBody] SubscriptionDTO subscriptionDto)
//        {
//            var subscription = _mapper.Map<SubscriptionModel>(subscriptionDto);

//            await _context.Subscription.AddAsync(subscription);

//            await _context.SaveChangesAsync();

//            var createdSubscription = await _context.Subscription.FindAsync(subscription.Id);

//            if (createdSubscription is null)
//            {
//                return BadRequest("Unable to create subscription");
//            }

//            return Ok(subscriptionDto);
//        }

//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [HttpPatch("{id}")]
//        public async Task<ActionResult> ChangeSubscription(int id, [FromBody] SubscriptionDTO subscriptiontDto)
//        {
//            var subscription = await _context.Subscription.FindAsync(id);

//            if (subscription is null)
//            {
//                return BadRequest(string.Format("Subscription with id {0} is not found", id));
//            }

//            SubscriptionMappingExtensions.ProjectFrom(subscription, subscriptiontDto);

//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<string>> DeleteSubscription(int id)
//        {
//            var subscription = await _context.Subscription.FindAsync(id);

//            if (subscription is null)
//            {
//                return BadRequest(string.Format("Subscription with id {0} is not found", id));
//            }

//            _context.Subscription.Remove(subscription);

//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
