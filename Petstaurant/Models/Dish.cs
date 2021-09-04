using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-30 characters")]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
            "Name should only contain word characters, hyphens, spaces and apostrophes")]
        public string Name { get; set; }
        [DisplayName("Food Group")]
        public int FoodGroupId { get; set; }
        public FoodGroup FoodGroup { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "You are allowed to use 5-200 characters")]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Range(0, 1000, ErrorMessage = "Choose a postive price")]
        public double Price { get; set; }
        [DisplayName("Cart Items")]
        public List<CartItem> CartItems { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.Today;
        public byte[] Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<Store> Store { get; set; }
    }
}
