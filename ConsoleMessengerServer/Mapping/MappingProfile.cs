using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer.Entities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegistrationDto>().ReverseMap();
            CreateMap<UserSearchResult, UserSearchResultDto>().ReverseMap();

            CreateMap<Dialog, DialogDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<ServerNetworkProvider, NetworkProviderDto>().ReverseMap();

            CreateMap<SuccessfulRegistrationResponse, SuccessfulRegistrationResponseDto>().ReverseMap();
        }

    }
}