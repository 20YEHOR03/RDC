using RDC.API.Models.Drone;

namespace RDC.API.Mapping
{
    public static class DroneMappingExtensions
    {
        public static void ProjectFrom(this DroneModel drone, DroneDTO droneDto)
        {

            drone.Model = droneDto.Model;
            drone.Name = droneDto.Name;
            drone.MaxWeight = droneDto.MaxWeight;
            drone.Info = droneDto.Info;
            drone.Type = droneDto.Type;
        }
    }
}
