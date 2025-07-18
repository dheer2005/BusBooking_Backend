namespace BusBooking.Models
{
    public class StateWiseCities
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
