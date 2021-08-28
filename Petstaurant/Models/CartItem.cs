using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Dish Dish { get; set; }

    }
}
