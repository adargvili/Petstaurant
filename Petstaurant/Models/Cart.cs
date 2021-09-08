using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Currency)]
        [Range(0, 10000, ErrorMessage = "Choose a postive price, not more than 10000")]
        [ConcurrencyCheck]
        public double TotalPrice { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }
        public User User { get; set; }
    }
}
