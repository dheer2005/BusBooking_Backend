namespace BusBooking.DTO
{
    public class BusRatingDTO
    {
        public int BookingId { get; set; }  
        public int BusId { get; set; }
        public int UserId { get; set; } 
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
