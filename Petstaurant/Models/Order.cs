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
        Standart,
        Express,
        Premium
    }
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
           "Country should only contain word characters, hyphens, spaces and apostrophes")]
        public string Country { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
           "City should only contain word characters, hyphens, spaces and apostrophes")]
        public string City { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        [RegularExpression(@"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}\s[a-zA-Z]{2,15}", ErrorMessage =
           "Please enter a real address")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Postal Code")]
        [StringLength(30, MinimumLength = 4)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        [StringLength(12, MinimumLength = 9)]
        public string PhoneNumber { get; set; }
        [DisplayName("Total Pay")]
        public double TotalPrice { get; set; }
        public Delivery Delivery { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; set; }
    }
}
