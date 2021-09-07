using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [DisplayName("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Range(1, 10, ErrorMessage = "You are allowed to order maximum of 10 items")]
        [DefaultValue(1)]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        [Range(0, 1000, ErrorMessage = "Choose a postive price")]
        public double Price { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
