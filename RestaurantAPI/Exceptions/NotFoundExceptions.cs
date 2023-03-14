using System;

namespace RestaurantAPI.Exceptions
{
    public class NotFoundExceptions : Exception
    {
        public NotFoundExceptions(string message) : base(message)
        {

        }
    }
}
