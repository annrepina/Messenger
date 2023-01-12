using DtoLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.Entities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegistrationDto>().ReverseMap();
            CreateMap<UserSearchResponse, UserSearchResponseDto>().ReverseMap();
            CreateMap<User, int>().ConvertUsing(source => source.Id);

            CreateMap<Dialog, DialogDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogRequestDto>().ForMember(dest => dest.UsersId, exp => exp.MapFrom(dial => dial.Users));
            CreateMap<CreateDialogRequestDto, Dialog>();
            CreateMap<CreateDialogResponse, CreateDialogResponseDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogResponse>().ForMember(dest => dest.DialogId, exp => exp.MapFrom(dial => dial.Id))
                                                     .ForMember(dest => dest.MessageId, exp => exp.MapFrom(dial => dial.Messages.First().Id)).ReverseMap();



            CreateMap<Message, MessageDto>().ReverseMap();

            CreateMap<ServerNetworkProvider, NetworkProviderDto>().ReverseMap();

            CreateMap<SuccessfulRegistrationResponse, SuccessfulRegistrationResponseDto>().ReverseMap();

        }
    }
}