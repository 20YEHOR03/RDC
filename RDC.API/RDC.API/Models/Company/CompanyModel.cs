using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
