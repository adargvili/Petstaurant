using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
            "Name should only contain word characters, hyphens, spaces and apostrophes")]
        public string Name { get; set; }
        public FoodGroup FoodGroup { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public List<CartItem> CartItems { get; set; }
        public DateTime Created { get; set; }
        [DisplayName("Image URL")]
        [DataType(DataType.ImageUrl)]
        [MaxLength(50)]
        public String Image { get; set; }
        public List<Store> Store { get; set; }
    }
}
