using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.User
{
    public class UserRegister
    {
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
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
