using Microsoft.AspNetCore.Mvc;

namespace BusBooking.DTO
{
    public class LoginRequestDTO
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
