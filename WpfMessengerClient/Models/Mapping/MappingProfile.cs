using AutoMapper;
using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();

            CreateMap<UserAccount, UserAccountDto>();

            CreateMap<Dialog, DialogDto>();

            CreateMap<Message, MessageDto>();
        }
    }
}
