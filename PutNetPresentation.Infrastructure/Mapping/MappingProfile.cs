using AutoMapper;
using PutNetPresentation.Core.Models;
using PutNetPresentation.Infrastructure.Dto;

namespace PutNetPresentation.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
        }
    }
}
