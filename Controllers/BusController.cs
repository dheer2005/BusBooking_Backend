using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : Controller
    {
        private readonly BusBookingDbContext _context;

        public BusController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddBus(BusDTO dto)
        {
            var bus = new Bus
            {
                BusName = dto.BusName,
                VendorName = dto.VendorName,
                TotalSeats = dto.TotalSeats,
                Price = dto.Price
            };
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return Ok(bus);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await _context.Buses.ToListAsync());
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            return bus != null ? Ok(bus) : NotFound();
        }

        [HttpPost("AddBusRating")]
        public async Task<IActionResult> AddRating(BusRatingDTO dto)
        {
            var busExists = await _context.Buses.AnyAsync(b => b.BusId == dto.BusId);
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);

            if (!busExists)
                return BadRequest($"Bus with ID {dto.BusId} does not exist.");

            if (!userExists)
                return BadRequest($"User with ID {dto.UserId} does not exist.");

            var existingRating = await _context.BusRatings
                .FirstOrDefaultAsync(r => r.BookingId == dto.BookingId && r.UserId == dto.UserId);

            if (existingRating != null)
                return BadRequest("You have already rated this bus.");

            var rating = new BusRating
            {   
                BookingId = dto.BookingId,
                BusId = dto.BusId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comments = dto.Comments
            };

            _context.BusRatings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }


        [HttpGet("GetBusRatings/{busId}")]
        public async Task<IActionResult> GetRatings(int busId)
        {
            var ratings = await _context.BusRatings.Where(r => r.BusId == busId).ToListAsync();
            return Ok(ratings);
        }
    }
}
