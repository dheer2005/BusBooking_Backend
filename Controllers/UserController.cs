using BusBooking.Context;
using BusBooking.DTO;
using BusBooking.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly BusBookingDbContext _context;

        public UserController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                EmailId = dto.EmailId,
                Role = dto.Role,
                Password = dto.Password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.EmailId == request.EmailId && x.Password == request.Password);
            return user != null ? Ok(user) : Unauthorized("Invalid credentials");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
    }
}
