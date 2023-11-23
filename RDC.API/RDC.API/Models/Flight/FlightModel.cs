using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.Flight
{
    public class FlightModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required] 
        public bool IsFragile { get; set; }
        [Required] 
        public double StartPosition { get; set; }
        [Required]
        public double EndPosition { get; set; }
        [Required] 
        public DateTime DateTime { get; set; }
        public string? Info { get; set; }
        [Required]
        public int DroneId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
