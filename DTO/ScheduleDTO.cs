namespace BusBooking.DTO
{
    public class ScheduleDTO
    {
        public int BusId { get; set; }
        public int ScheduleId { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
