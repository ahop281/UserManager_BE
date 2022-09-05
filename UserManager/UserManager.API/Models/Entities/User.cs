﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UserManager.API.Models.Entities
{
    public class User : BaseEntity
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
    }
}
