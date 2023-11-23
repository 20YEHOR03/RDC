namespace RDC.API.Models.Flight
{
    public class FlightDTO
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public bool IsFragile { get; set; }
        public double StartPosition { get; set; }
        public double EndPosition { get; set; }
        public DateTime DateTime { get; set; }
        public string? Info { get; set; }
        public int DroneId { get; set; }
    }
}
