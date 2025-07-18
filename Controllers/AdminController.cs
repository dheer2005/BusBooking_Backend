using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly BusBookingDbContext _context;

        public AdminController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("buses")]
        public async Task<IActionResult> GetAllBuses()
        {
            var buses = await _context.Buses.ToListAsync();
            return Ok(buses);
        }

        [HttpPost("bus")]
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

        [HttpPut("bus/{id}")]
        public async Task<IActionResult> UpdateBus(int id, BusDTO dto)
        {
            var existing = await _context.Buses.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.BusName = dto.BusName;
            existing.VendorName = dto.VendorName;
            existing.TotalSeats = dto.TotalSeats;
            existing.Price = dto.Price;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("bus/{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return NotFound();

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("schedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _context.Schedules
            .Include(s => s.Bus)
            .Include(s => s.FromLocation)
            .Include(s => s.ToLocation)
            .Select(s => new
            {
                s.ScheduleId,
                s.BusId,
                BusName = s.Bus.BusName,
                s.FromLocationId,
                FromLocation = s.FromLocation.LocationName,
                s.ToLocationId,
                ToLocation = s.ToLocation.LocationName,
                s.TravelDate,
                s.DepartureTime,
                s.ArrivalTime
            })
            .ToListAsync();

            return Ok(schedules);
        }

        [HttpPost("schedule")]
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

        [HttpPut("schedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, ScheduleDTO dto)
        {
            var existing = await _context.Schedules.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.BusId = dto.BusId;
            existing.FromLocationId = dto.FromLocationId;
            existing.ToLocationId = dto.ToLocationId;
            existing.TravelDate = dto.TravelDate;
            existing.DepartureTime = dto.DepartureTime;
            existing.ArrivalTime = dto.ArrivalTime;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("schedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.FromLocation)
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.ToLocation)
                .Include(b => b.BusBookingPassengers)
                .ToListAsync();

            var result = bookings.Select(b => new
            {
                b.BookingId,
                b.BookingDate,
                TravelDate = b.Schedule?.TravelDate,
                CustomerName = b.User?.FullName,
                BusName = b.Schedule?.Bus?.BusName,
                From = b.Schedule?.FromLocation?.LocationName,
                To = b.Schedule?.ToLocation?.LocationName,
                TotalAmount = (b.Schedule?.Bus.Price ?? 0) * b.BusBookingPassengers.Count,
                Passengers = b.BusBookingPassengers.Select(p => new
                {
                    p.PassengerName,
                    p.Age,
                    p.Gender,
                    p.SeatNo
                })
            });

            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalRevenue = await _context.Bookings
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .SumAsync(b => b.BusBookingPassengers.Count * b.Schedule.Bus.Price);

            return Ok(new
            {
                TotalUsers = await _context.Users.Where(x=>x.Role != "Admin").CountAsync(),
                TotalBuses = await _context.Buses.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalPassengers = await _context.Passengers.CountAsync(),
                TotalRevenue = totalRevenue
            });
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings()
        {
            var allRatings = await _context.BusRatings.ToListAsync();
            return Ok(allRatings);
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var paymentList = await _context.payments.ToListAsync();
            return Ok(paymentList);
        }

        [HttpGet("dashboard/topFiveRevenuebuses")]
        public async Task<IActionResult> GetTopFiveRevenueBus()
        {
            var revenueData = await _context.Bookings
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .GroupBy(b => new { b.Schedule.Bus.BusName })
                .Select(group => new
                {
                    BusName = group.Key.BusName,
                    Revenue = group.Sum(b => b.BusBookingPassengers.Count * b.Schedule.Bus.Price)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToListAsync();

            return Ok(revenueData);
        }

        [HttpGet("dashboard/leastFiveRevenueBuses")]
        public async Task<IActionResult> GetLeastFiveRevenueBuses()
        {
            var revenueData = await _context.Bookings
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .GroupBy(b => new { b.Schedule.Bus.BusName })
                .Select(group => new
                {
                    BusName = group.Key.BusName,
                    Revenue = group.Sum(b => b.BusBookingPassengers.Count * b.Schedule.Bus.Price)
                })
                .OrderBy(x => x.Revenue)
                .Take(5)
                .ToListAsync();

            return Ok(revenueData);
        }

        [HttpGet("dashboard/top-buses")]
        public async Task<IActionResult> GetTopBuses()
        {
            var topBuses = await _context.Bookings
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .GroupBy(b => new { b.Schedule.Bus.BusName })
                .Select(group => new
                {
                    BusName = group.Key.BusName,
                    TotalBookings = group.Count()
                })
                .OrderByDescending(x=>x.TotalBookings)
                .Take(5)
                .ToListAsync();

            return Ok(topBuses);
        }

        [HttpGet("dashboard/top-loved-buses")]
        public async Task<IActionResult> GetTopThreeMostLovedBuses()
        {
            var topRatedBuses = await _context.BusRatings
                .GroupBy(r => r.BusId)
                .Select(g => new
                {
                    BusId = g.Key,
                    AverageRating = g.Average(r => r.Rating),
                    RatingCount = g.Count()
                })
                .Where(r => r.AverageRating >= 4)
                .OrderByDescending(r => r.AverageRating)
                .ThenByDescending(r => r.RatingCount)
                .Take(3)
                .ToListAsync();

            var scheduleInfo = await _context.Schedules
                .Include(s => s.Bus)
                .Where(s => topRatedBuses.Select(r => r.BusId).Contains(s.BusId))
                .GroupBy(s => s.BusId)
                .Select(g => g
                    .OrderByDescending(s => s.TravelDate) 
                    .Select(s => new
                    {
                        s.Bus.BusId,
                        s.Bus.BusName,
                        s.Bus.VendorName,
                        s.Bus.Price,
                        s.TravelDate,
                        s.DepartureTime,
                        s.ArrivalTime
                    }).FirstOrDefault())
                .ToListAsync();

            var result = scheduleInfo
                .Join(topRatedBuses,
                      schedule => schedule.BusId,
                      rating => rating.BusId,
                      (schedule, rating) => new
                      {
                          schedule.BusId,
                          schedule.BusName,
                          schedule.VendorName,
                          schedule.Price,
                          schedule.TravelDate,
                          schedule.DepartureTime,
                          schedule.ArrivalTime,
                          AverageRating = Math.Round(rating.AverageRating, 1),
                          NumberOfReviews = rating.RatingCount
                      })
                .ToList();

            return Ok(result);
        }
    }
}


