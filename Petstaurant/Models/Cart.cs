using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [DisplayName("Cart Items")]
        public List<CartItem> CartItems { get; set; }
        [DisplayName("Total Price")]
        public double TotalPrice { get; set; }
        [ForeignKey("User")]
        [DisplayName("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }
    }
}
