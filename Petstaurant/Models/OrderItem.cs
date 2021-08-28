using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(1)]
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Dish Dish { get; set; }
    }
}
