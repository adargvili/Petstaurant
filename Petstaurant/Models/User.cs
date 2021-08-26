﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Petstaurant.Models
{

    public enum UserType
    {
        Admin,
        Customer
    }
    public enum Gender
    {
        DontWishToSpecify,
        Male,
        Female
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Email Adress")]
        [EmailAddress]
        [StringLength(30, MinimumLength = 5)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage =
        "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        public Gender Gender { get; set; } = Gender.DontWishToSpecify;
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage =
            "Name should only contain word characters, hyphens, spaces and apostrophes")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Registered { get; set; } = DateTime.Today;
        public List<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public UserType UserType { get; set; } = UserType.Customer;


    }
}
