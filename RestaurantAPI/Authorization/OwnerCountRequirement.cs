using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class OwnerCountRequirement : IAuthorizationRequirement
    {
        public int RestaurantNumbers { get; }

        public OwnerCountRequirement(int restaurantNumbers)
        {
            RestaurantNumbers = restaurantNumbers;
        }
    }
}
