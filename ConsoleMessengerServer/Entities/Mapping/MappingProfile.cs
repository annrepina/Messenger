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

            CreateMap<Dialog, DialogDto>().ForMember(dest => dest.UsersData, exp => exp.MapFrom(d => d.UsersData)).ReverseMap();

            CreateMap<Message, MessageDto>().ForMember(dest => dest.SenderUserData, exp => exp.MapFrom(m => m.UserData)).ReverseMap();

            CreateMap<ServerNetworkProviderEntity, NetworkProviderDto>().ReverseMap();
            CreateMap<ServerNetworkProvider, ServerNetworkProviderEntity>().ReverseMap();


            CreateMap<UserData, RegistrationDto>().ForMember(dest => dest.Name, exp => exp.MapFrom(user => user.Person.Name))
                                                  .ForMember(dest => dest.PhoneNumber, exp => exp.MapFrom(user => user.Person.PhoneNumber)).ReverseMap();      

            //CreateMap<RegistrationDto, UserData>().ForMember(dest => dest.Person.PhoneNumber, exp => exp.MapFrom(reg =>  reg.PhoneNumber))
            //                                      .ForMember(dest => dest.Person.Name, exp => exp.MapFrom(reg => reg.Name)).ReverseMap();  
        }

    }
}
