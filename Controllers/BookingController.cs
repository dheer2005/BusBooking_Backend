using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly BusBookingDbContext _context;

        public BookingController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookingDTO dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
                return BadRequest("User does not exist. Please register or login again.");

            var booking = new Booking
            {
                CustId = dto.UserId,
                ScheduleId = dto.ScheduleId,
                BookingDate = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            foreach (var p in dto.BusBookingPassengers)
            {
                var passenger = new Passenger
                {
                    BookingId = booking.BookingId,
                    PassengerName = p.PassengerName,
                    Age = p.Age,
                    Gender = p.Gender,
                    SeatNo = p.SeatNo
                };
                _context.Passengers.Add(passenger);
            }

            await _context.SaveChangesAsync();
            return Ok(new
            {
                booking.BookingId,
                booking.BookingDate,
                booking.ScheduleId,
                booking.CustId
            });
        }

        [HttpDelete("cancelBooking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.BusBookingPassengers)
                .Include(b => b.Schedule)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return NotFound("Booking not found.");

            var departureTime = booking.Schedule.TravelDate.Date
                .Add(new TimeSpan(booking.Schedule.DepartureTime.Hour, booking.Schedule.DepartureTime.Minute, booking.Schedule.DepartureTime.Second));

            if (DateTime.UtcNow.AddHours(5.5) > departureTime.AddHours(-3))
            {
                return BadRequest("You can only cancel a booking at least 3 hours before the journey starts.");
            }

            _context.Passengers.RemoveRange(booking.BusBookingPassengers);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking cancelled successfully." });
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var ratings = await _context.BusRatings
            .Where(r => r.UserId == userId)
            .ToListAsync();

            var bookings = await _context.Bookings
             .Where(b => b.CustId == userId)
             .Include(b => b.BusBookingPassengers)
             .Include(b => b.Schedule)
                 .ThenInclude(s => s.Bus)
             .Include(b => b.Schedule)
                 .ThenInclude(s => s.FromLocation)
             .Include(b => b.Schedule)
                 .ThenInclude(s => s.ToLocation)
             .ToListAsync();

            var result = bookings.Select(b => new BookingResponseDTO
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate,
                ScheduleId = b.ScheduleId,
                TravelDate = b.Schedule.TravelDate,
                DepartureTime = b.Schedule.DepartureTime,
                ArrivalTime = b.Schedule.ArrivalTime,
                BusId = b.Schedule?.Bus?.BusId ?? 0,
                BusName = b.Schedule?.Bus?.BusName ?? "N/A",
                From = b.Schedule?.FromLocation?.LocationName ?? "N/A",
                To = b.Schedule?.ToLocation?.LocationName ?? "N/A",
                TotalAmount = (b.Schedule?.Bus?.Price ?? 0) * b.BusBookingPassengers.Count,
                IsCompleted = (b.Schedule?.ArrivalTime ?? DateTime.MaxValue) < DateTime.Now,
                HasRated = ratings.Any(r => r.BookingId == b.BookingId),
                Passengers = b.BusBookingPassengers.Select(p => new PassengerDTO
                {
                    PassengerName = p.PassengerName,
                    Age = p.Age,
                    Gender = p.Gender,
                    SeatNo = p.SeatNo
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> Get(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            return booking != null ? Ok(booking) : NotFound();
        }

        [HttpGet("GetBookedSeats")]
        public async Task<IActionResult> GetBookedSeats(int scheduleId)
        {
            var seats = await _context.Passengers
                .Where(p => _context.Bookings.Any(b => b.BookingId == p.BookingId && b.ScheduleId == scheduleId))
                .Select(p => p.SeatNo)
                .ToListAsync();
            return Ok(seats);
        }

    }
}
