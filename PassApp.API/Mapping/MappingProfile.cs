using AutoMapper;
using PassApp.API.Dtos;
using PassApp.API.Models;

namespace PassApp.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserInfoDto>();
            CreateMap<PasswordRegister, Password>();    
            CreateMap<Password, PasswordReturn>().ReverseMap();  
            CreateMap<PasswordUpdate, Password>();        
        }
    }
}