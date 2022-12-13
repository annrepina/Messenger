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
            CreateMap<PersonModel, PersonDto>().ReverseMap();

            CreateMap<UserAccountModel, UserAccountDto>().ReverseMap();

            CreateMap<DialogModel, DialogDto>().ReverseMap();

            CreateMap<MessageModel, MessageDto>().ReverseMap();

            CreateMap<FrontClient, ClientDto>().ReverseMap();
        }
    }
}
