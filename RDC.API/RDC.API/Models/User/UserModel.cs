using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public bool IsMain { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
