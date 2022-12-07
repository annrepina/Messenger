using AutoMapper;
using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Mapping
{
    public class DialogMappingProfile : Profile
    {
        public DialogMappingProfile()
        {
            CreateMap<Dialog, DialogDto>();
        }
    }
}
