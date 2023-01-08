using AutoMapper;
using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Services;

namespace WpfMessengerClient.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Dialog, DialogDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<ClientNetworkProvider, NetworkProviderDto>().ReverseMap();

            CreateMap<RegistrationData, RegistrationDto>().ReverseMap();
         
            CreateMap<RegistrationData, User>().ReverseMap();
        }
    }
}