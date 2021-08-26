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
        public string Name { get; set; }
        public FoodGroup FoodGroup { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public List<CartItem> CartItems { get; set; }
        public DateTime Created { get; set; }
        [DisplayName("Image File")]
        [DataType(DataType.ImageUrl)]
        public String Image { get; set; }
        public List<Store> Store { get; set; }
    }
}
