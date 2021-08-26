using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Store
    {
        public int Id { get; set; }
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
           "Country name should only contain word characters, hyphens, spaces and apostrophes")]
        [StringLength(30, MinimumLength = 2)]
        public string Country { get; set; }
        [RegularExpression( @"\d{1,3}.?\d{0,3}\s[a-zA-Z]{2,30}\s[a-zA-Z]{2,15}", ErrorMessage =
           "Please enter a real address")]
        [StringLength(30, MinimumLength = 4)]
        public string Address { get; set; }
        public List<Dish> Dish { get; set; }

    }
}
