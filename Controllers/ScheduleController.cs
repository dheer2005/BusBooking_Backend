using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private readonly BusBookingDbContext _context;

        public ScheduleController(BusBookingDbContext context)
        {
            _context = context;
        }
        

        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleDTO dto)
        {
            var schedule = new Schedule
            {
                BusId = dto.BusId,
                FromLocationId = dto.FromLocationId,
                ToLocationId = dto.ToLocationId,
                TravelDate = dto.TravelDate,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _context.Schedules
            .Include(s => s.Bus)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

            if (schedule == null)
                return NotFound();

            var response = new
            {
                schedule.ScheduleId,
                schedule.BusId,
                schedule.TravelDate,
                DepartureTime = schedule.DepartureTime.ToString("HH:mm:ss"),
                ArrivalTime = schedule.ArrivalTime.ToString("HH:mm:ss"),
                TotalSeats = schedule.Bus.TotalSeats,
                Price = schedule.Bus.Price
            };

            return Ok(response);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(int fromLocation, int toLocation, DateTime travelDate)
    {
            var busRatings = await _context.BusRatings
                .GroupBy(r => r.BusId)
                .Select(g => new
                {
                    BusId = g.Key,
                    AverageRating = g.Any() ? g.Average(r => r.Rating) : 0,
                    RatingCount = g.Count()
                })
                .ToListAsync();

            var results = await _context.Schedules
                .Include(s => s.Bus)
                .Include(s => s.FromLocation)
                .Include(s => s.ToLocation)
                .Where(s => s.FromLocationId == fromLocation &&
                            s.ToLocationId == toLocation &&
                            s.TravelDate.Date == travelDate.Date)
                .Select(s => new
                {
                    s.ScheduleId,
                    s.TravelDate,
                    s.DepartureTime,
                    s.ArrivalTime,
                    s.Bus.BusId,
                    s.Bus.BusName,
                    s.Bus.VendorName,
                    s.Bus.TotalSeats,
                    s.Bus.Price,
                    FromLocation = s.FromLocation.LocationName,
                    ToLocation = s.ToLocation.LocationName,
                    BookedSeats = _context.Passengers
                        .Where(p => _context.Bookings.Any(b => b.BookingId == p.BookingId && b.ScheduleId == s.ScheduleId))
                        .Count()
                })
                .ToListAsync();

            var resultWithAvailableSeats = results.Select(r =>
            {
                var ratingInfo = busRatings.FirstOrDefault(br => br.BusId == r.BusId);
                double avgRating = ratingInfo?.AverageRating ?? 0;
                int ratingCount = ratingInfo?.RatingCount ?? 0;

                return new
                {
                    r.ScheduleId,
                    r.TravelDate,
                    r.DepartureTime,
                    r.ArrivalTime,
                    r.BusName,
                    r.VendorName,
                    r.TotalSeats,
                    r.Price,
                    r.FromLocation,
                    r.ToLocation,
                    BookedSeats = r.BookedSeats,
                    AvailableSeats = r.TotalSeats - r.BookedSeats,
                    AverageRating = Math.Round(avgRating, 1),
                    RatingCount = ratingCount,
                    Duration = (r.ArrivalTime - r.DepartureTime).TotalMinutes > 0
                        ? $"{(int)(r.ArrivalTime - r.DepartureTime).TotalHours}h {(int)(r.ArrivalTime - r.DepartureTime).Minutes}m"
                        : "N/A"
                };
            });

            return Ok(resultWithAvailableSeats);
        }


        [HttpGet("GetBusLocations")]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _context.locations.ToListAsync());
        }
    }
}
