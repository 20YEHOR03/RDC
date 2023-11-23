using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.Drone
{
public class DroneModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Model { get; set; }
    [Required] 
    public string Type { get; set; }
    [Required]
    public double MaxWeight { get; set; }
    public string? Info { get; set; }
    [Required] 
    public int CompanyId { get; set; }
}
}
