using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public enum Delivery
    {
        Standard,
        Express
    }
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-30 characters")]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
           "Country should only contain word characters, hyphens, spaces and apostrophes")]
        public string Country { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "You are allowed to use only 5-30 characters")]
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage =
           "Please do not use specialized sybmols in the address")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Postal Code")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "You are allowed to use only 4-30 characters")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "You are allowed to use only 9-12 numbers")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Total Price")]
        [Range(0, 10000, ErrorMessage = "Choose a postive price")]
        public double TotalPrice { get; set; }
        public Delivery Delivery { get; set; } = Delivery.Standard;
        [DisplayName("User")]
        [ForeignKey("User")]
        public string UserName { get; set; }
        public User User { get; set; }
        [DisplayName("Order Items")]
        public List<OrderItem> OrderItems { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}
