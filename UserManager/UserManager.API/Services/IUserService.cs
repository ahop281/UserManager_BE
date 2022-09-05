using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.API.Models.DTO;

namespace UserManager.API.Services
{
    public interface IUserService
    {
        public Task<List<UserDTO>> GetAllUsers();
        public Task<UserDTO> GetUser(Guid id);
        public Task<UserDTO> AddUser(AddUserRequest user);
        public Task<UserDTO> UpdateUser(UpdateUserRequest user);
        public Task<UserDTO> DeleteUser(Guid id);
    }
}
