using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class OwnerCountRequirementHandler : AuthorizationHandler<OwnerCountRequirement>
    {
        private readonly RestaurantDbContext dbContext;

        public OwnerCountRequirementHandler(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerCountRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdRestaurantsCount = dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (createdRestaurantsCount >= requirement.RestaurantNumbers)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
