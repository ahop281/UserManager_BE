using AutoMapper;
using UserManager.API.Models.DTO;
using UserManager.API.Models.Entities;

namespace UserManager_BE.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>()
                .ReverseMap();
            CreateMap<AddUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
        }
    }
}
