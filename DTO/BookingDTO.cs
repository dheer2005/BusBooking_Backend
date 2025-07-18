namespace BusBooking.DTO
{
    public class BookingDTO
    {
        public int UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public int ScheduleId { get; set; }
        public List<PassengerDTO> BusBookingPassengers { get; set; }
    }
}
