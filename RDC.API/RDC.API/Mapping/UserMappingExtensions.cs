using RDC.API.Models.User;

namespace RDC.API.Mapping
{
    public static class UserMappingExtensions
    {
        public static void ProjectFrom(this UserModel user, UserDTO userDto)
        {
            user.Name = userDto.Name;
            user.Email = userDto.Email;
        }
    }
}
