using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Petstaurant.Models;

namespace Petstaurant.Data
{
    public class PetstaurantContext : DbContext
    {
        public PetstaurantContext (DbContextOptions<PetstaurantContext> options)
            : base(options)
        {
        }

        public DbSet<Petstaurant.Models.Cart> Cart { get; set; }

        public DbSet<Petstaurant.Models.CartItem> CartItem { get; set; }

        public DbSet<Petstaurant.Models.Dish> Dish { get; set; }

        public DbSet<Petstaurant.Models.FoodGroup> FoodGroup { get; set; }

        public DbSet<Petstaurant.Models.Order> Order { get; set; }

        public DbSet<Petstaurant.Models.OrderItem> OrderItem { get; set; }

        public DbSet<Petstaurant.Models.Store> Store { get; set; }

        public DbSet<Petstaurant.Models.User> User { get; set; }
    }
}
