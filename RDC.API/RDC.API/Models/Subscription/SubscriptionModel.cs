using System.ComponentModel.DataAnnotations;

namespace RDC.API.Models.Subscription
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int AccountCount { get; set; }
    }
}
