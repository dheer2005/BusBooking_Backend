namespace BusBooking.Models
{
    public class Bus
    {
        public int BusId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
