﻿using System;
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
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
           "Country name should only contain word characters, hyphens, spaces and apostrophes")]
        [StringLength(30, MinimumLength = 2)]
        public string Country { get; set; }
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+$", ErrorMessage =
           "Please do not use specialized sybmols in the address")]
        [StringLength(30, MinimumLength = 4)]
        public string Address { get; set; }
        [Required]
        [DisplayName("Postal Code")]
        [StringLength(30, MinimumLength = 4)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        public List<Dish> Dish { get; set; }

    }
}
