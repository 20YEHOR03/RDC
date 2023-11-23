using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.User
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
