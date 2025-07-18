namespace BusBooking.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }
        public Bus Bus { get; set; }

        public int FromLocationId { get; set; }
        public Location FromLocation { get; set; }

        public int ToLocationId { get; set; }
        public Location ToLocation { get; set; }

        public DateTime TravelDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
