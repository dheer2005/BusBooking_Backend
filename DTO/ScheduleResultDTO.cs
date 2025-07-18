namespace BusBooking.DTO
{
    public class ScheduleResultDTO
    {
        public int ScheduleId { get; set; }
        public string BusName { get; set; }
        public string VendorName { get; set; }
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

