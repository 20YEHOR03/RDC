using RDC.API.Models.Subscription;

namespace RDC.API.Mapping
{
    public static class SubscriptionMappingExtensions
    {
        public static void ProjectFrom(this SubscriptionModel subscription, SubscriptionDTO subscriptiontDto)
        {
            subscription.Name = subscriptiontDto.Name;
            subscription.Price = subscriptiontDto.Price;
            subscription.AccountCount = subscriptiontDto.AccountCount;
        }
    }
}
