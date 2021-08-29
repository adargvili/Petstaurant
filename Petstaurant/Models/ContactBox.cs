using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Petstaurant.Models
{
    public class ContactBox
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
            "Name should only contain word characters, hyphens, spaces and apostrophes")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-30 characters")]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "You are allowed to use only 4-30 characters")]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "You are allowed to use only 5-30 characters")]
        public string Subject { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "You are allowed to use only 2-200 characters")]
        public string Body { get; set; }
    }
}
