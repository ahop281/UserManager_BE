using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.API.Models.DTO;
using UserManager.API.Models.Entities;
using UserManager.API.Repositories;

namespace UserManager.API.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _context;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                var users = await _context.GetAllUsers();
                var usersDto = _mapper.Map<List<UserDTO>>(users);
                return usersDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> GetUser(Guid id)
        {
            try
            {
                var user = await _context.GetUser(id);
                var UserDTO = _mapper.Map<UserDTO>(user);
                return UserDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> AddUser(AddUserRequest user)
        {
            try
            {
                var userEntity = _mapper.Map<User>(user);
                userEntity.Id = Guid.NewGuid();

                var result = await _context.AddUser(userEntity);
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> UpdateUser(UpdateUserRequest user)
        {
            try
            {
                var userExisted = await _context.GetUser(user.Id);

                if (userExisted == null)
                    return null;

                var userEntity = _mapper.Map<User>(user);
                userEntity.CreatedAt = userExisted.CreatedAt;

                var result = await _context.UpdateUser(userEntity);
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> DeleteUser(Guid id)
        {
            try
            {
                var user = await _context.GetUser(id);

                if (user == null)
                    return null;

                var result = await _context.DeleteUser(user);
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
