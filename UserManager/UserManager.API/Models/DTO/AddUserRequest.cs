using System;

namespace UserManager.API.Models.DTO
{
    public class AddUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
    }
}
