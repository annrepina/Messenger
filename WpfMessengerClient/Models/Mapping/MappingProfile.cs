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
            CreateMap<Person, PersonDto>().ReverseMap();

            CreateMap<UserData, UserDataDto>().ReverseMap();

            CreateMap<Dialog, DialogDto>().ForMember(dest => dest.UsersData, exp => exp.MapFrom(d => d.UserDataCollection)).ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<ClientNetworkProvider, NetworkProviderDto>().ReverseMap();

            CreateMap<RegistrationData, RegistrationDto>().ReverseMap();

            //CreateMap<RegistrationData, UserData>().ForMember(dest => dest.Person.Name, exp => exp.MapFrom(reg => reg.Name))
            //                                       .ForMember(dest => dest.Person.PhoneNumber, exp => exp.MapFrom(reg => reg.PhoneNumber));

            CreateMap<RegistrationData, UserData>().ForPath(dest => dest.Person.Name, exp => exp.MapFrom(reg => reg.Name))
                                                   .ForPath(dest => dest.Person.PhoneNumber, exp => exp.MapFrom(reg => reg.PhoneNumber)).ReverseMap();

            //CreateMap<UserData, SuccessfulRegistrationDto>().ForMember(nameof(SuccessfulRegistrationDto.PhoneNumber), exp => exp.MapFrom(c => c.Person.PhoneNumber)).ReverseMap();
        }
    }
}
