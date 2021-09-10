using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class FoodGroup
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
            "Please enter a valid food group name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-30 characters")]
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
