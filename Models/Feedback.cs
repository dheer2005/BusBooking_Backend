namespace BusBooking.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public string Comments { get; set; } = string.Empty;
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
    }
}
