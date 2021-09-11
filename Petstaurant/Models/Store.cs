using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Store
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-20 characters")]
        [RegularExpression(@"[a-zA-Z]+(?:[ '-][a-zA-Z]+)*", ErrorMessage =
           "Please choose a valid city name")]
        public string City { get; set; }
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage =
           "Israeli address format is required (Example: Israel Galili 5)")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "You are allowed to use only 4-30 characters")]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^\d{7}$", ErrorMessage =
           "Israeli postal code format is required")]
        [DisplayName("Postal Code")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "You are allowed to use only 7 digits")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [DisplayName("Dishes")]
        public List<Dish> Dish { get; set; }
        public List<Order> Orders { get; set; }

    }
}
