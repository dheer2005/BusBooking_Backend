using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly BusBookingDbContext _context;

        public LocationController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var states = await _context.StateWiseCities
                .Select(s => s.State.Trim())
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            return Ok(states);
        }

        [HttpGet("districts/{state}")]
        public async Task<IActionResult> GetDistricts(string state)
        {
            var districts = await _context.StateWiseCities
                .Where(s => s.State == state)
                .Select(s => s.District)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            return Ok(districts);
        }

        [HttpGet("cities/{state}/{district}")]
        public async Task<IActionResult> GetCities(string state, string district)
        {
            var addedCities = await _context.locations
                .Select(l => l.LocationName.Trim()) 
                .ToListAsync();

            var filteredCities = await _context.StateWiseCities
                .Where(s => s.State == state && s.District == district && !addedCities.Contains(s.City))
                .Select(s => s.City)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(filteredCities);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Location location)
        {
            _context.locations.Add(location);
            await _context.SaveChangesAsync();
            return Ok(location);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Locations = await _context.locations.ToListAsync();
            return Ok(Locations);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Location updatedLocation)
        {
            var location = await _context.locations.FindAsync(id);
            if (location == null)
                return NotFound();

            location.LocationName = updatedLocation.LocationName;
            await _context.SaveChangesAsync();
            return Ok(location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.locations.FindAsync(id);
            if (location == null)
                return NotFound();

            _context.locations.Remove(location);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
