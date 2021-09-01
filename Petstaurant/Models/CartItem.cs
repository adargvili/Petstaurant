using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [DisplayName("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        [DefaultValue(1)]
        [Range(1, 10, ErrorMessage = "You are allowed to order maximum of 10 items")]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        [Range(0, 1000, ErrorMessage = "Choose a postive price")]
        public double Price { get; set; }
        [DisplayName("Dish")]
        public int DishId { get; set; }
        public Dish Dish { get; set; }

    }
}
