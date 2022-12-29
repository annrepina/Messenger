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

            CreateMap<UserAccount, UserAccountDto>().ReverseMap();

            CreateMap<Dialog, DialogDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<BackClient, Client>().ReverseMap();

            CreateMap<UserAccount, RegistrationAuthentificationDto>().ForMember(nameof(RegistrationAuthentificationDto.PhoneNumber), exp => exp.MapFrom(c => c.Person.PhoneNumber)).ReverseMap();

            CreateMap<UserAccount, SuccessfulRegistrationDto>().ForMember(nameof(SuccessfulRegistrationDto.PhoneNumber), exp => exp.MapFrom(c => c.Person.PhoneNumber))
                                                               .ForMember(nameof(SuccessfulRegistrationDto.ClientId), exp => exp.MapFrom(c => c.Clients.FirstOrDefault())).ReverseMap();
        }

    }
}
