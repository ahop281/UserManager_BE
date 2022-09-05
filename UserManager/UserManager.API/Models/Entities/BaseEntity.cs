using System;
using System.ComponentModel.DataAnnotations;

namespace UserManager.API.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
