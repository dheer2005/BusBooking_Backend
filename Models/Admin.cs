namespace BusBooking.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
