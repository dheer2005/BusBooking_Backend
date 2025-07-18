namespace BusBooking.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string PassengerName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int SeatNo { get; set; }
    }
}
