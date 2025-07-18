namespace BusBooking.DTO
{
    public class BookingResponseDTO
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int ScheduleId { get; set; }
        public int BusId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasRated { get; set; }
        public List<PassengerDTO> Passengers { get; set; } = new();
    }
}
