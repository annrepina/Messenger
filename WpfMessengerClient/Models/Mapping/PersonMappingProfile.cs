using AutoMapper;
using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Mapping
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<PersonModel, PersonDto>();
        }
    }


    //public class PersonMappingProfile<Tsource, TDestination> : Profile
    //where Tsource : class
    //where TDestination : class
    //{
    //    public PersonMappingProfile()
    //    {
    //        CreateMap<Tsource, TDestination>();
    //    }
    //}
}
