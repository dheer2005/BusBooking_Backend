namespace BusBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustId { get; set; }
        public User User { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public DateTime BookingDate { get; set; }

        public ICollection<Passenger> BusBookingPassengers { get; set; }
    }
}
