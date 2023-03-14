using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        void Delete(int restaurantId, int dishId);
        void DeleteAll(int restaurantId);
        List<DishDto> GetAll(int restaurantId);
        DishDto GetById(int restaurantId, int dishId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext context;
        private readonly IMapper mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            context.Dishes.Add(dishEntity);
            context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            Dish dish = context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if(dish is null || dish.RestaurantId != restaurantId)
                throw new NotFoundExceptions("Dish not found");

            var dishDto = mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var dishDtos = mapper.Map<List<DishDto>>(GetRestaurantById(restaurantId).Dishes);
            return dishDtos;
        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            context.RemoveRange(restaurant.Dishes);
            context.SaveChanges();
        }

        public void Delete(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null)
                throw new NotFoundExceptions("Dish not found");

            context.Remove(dish);
            context.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            Restaurant restaurant = context
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundExceptions("Restaurant nor found");

            return restaurant;
        }
    }
}
