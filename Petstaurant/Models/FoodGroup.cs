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
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
