namespace BusBooking.Models
{
    public class BusRating
    {
        public int BusRatingId { get; set; }
        public int BookingId { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int Rating { get; set; } 
        public string Comments { get; set; } = string.Empty;
        public DateTime RatingDate { get; set; } = DateTime.Now;
    }
}
