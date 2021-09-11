using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Credit Card")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "You are allowed to use only 16 characters")]
        [RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$", ErrorMessage =
           "Credit card format is required")]
        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; }
        [DisplayName("Store")]
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "You are allowed to use only 5-50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9, ]*$", ErrorMessage =
           "Israeli address format is required (Example: Israel Galili 5)")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Postal Code")]
        [RegularExpression(@"^\d{7}$", ErrorMessage =
           "Israeli postal code format is required")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "You are allowed to use only 7 digits")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Mobile Phone Number")]
        [RegularExpression(@"^0(?:[234689]|5[0-689]|7[246789])(?![01])(\d{7})$", ErrorMessage =
           "Israeli phone number format is required")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "You are allowed to use only 9-10 numbers")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Total Price")]
        [Range(0, 10000, ErrorMessage = "Only a postive price, not more than 10000")]
        public double TotalPrice { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }
        public User User { get; set; }
        [DisplayName("Order Items")]
        public List<OrderItem> OrderItems { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Order Time")]
        public DateTime OrderTime { get; set; } = DateTime.Now;

    }
}
