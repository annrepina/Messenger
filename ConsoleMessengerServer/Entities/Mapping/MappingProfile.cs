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
            CreateMap<Person, PersonDto>().ReverseMap();

            CreateMap<UserData, UserDataDto>().ReverseMap();

            CreateMap<Dialog, DialogDto>().ForMember(dest => dest.UsersData, exp => exp.MapFrom(d => d.UsersDataList)).ReverseMap();

            CreateMap<Message, MessageDto>().ForMember(dest => dest.SenderUserData, exp => exp.MapFrom(m => m.UserData)).ReverseMap();

            CreateMap<ServerNetworkProviderEntity, NetworkProviderDto>().ReverseMap();

            CreateMap<UserData, RegistrationAuthentificationDto>().ForMember(dest => dest.PhoneNumber, exp => exp.MapFrom(c => c.Person.PhoneNumber)).ReverseMap();

            //CreateMap<UserData, SuccessfulRegistrationDto>().ForMember(nameof(SuccessfulRegistrationDto.PhoneNumber), exp => exp.MapFrom(c => c.Person.PhoneNumber))
            //                                                   .ForMember(nameof(SuccessfulRegistrationDto.NetworkProviderId), exp => exp.MapFrom(c => c.NetworkProviders.FirstOrDefault())).ReverseMap();
        }

    }
}
