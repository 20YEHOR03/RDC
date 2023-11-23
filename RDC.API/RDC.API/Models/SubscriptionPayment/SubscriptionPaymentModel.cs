using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.SubscriptionPayment
{
    public class SubscriptionPaymentModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime DurationDate { get; set; }
        [Required]
        public int SubscriptionId { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
