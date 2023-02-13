using AutoMapper;
using UserManager.API.Models.DTO;
using UserManager.API.Models.Entities;
using UserManager.API.Models.Request;
using UserManager.API.Models.Response;

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

            CreateMap<SignUpRequest, User>();
            CreateMap<User, SignUpResponse>();
        }
    }
}
