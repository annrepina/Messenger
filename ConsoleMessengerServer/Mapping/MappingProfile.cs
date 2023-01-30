using CommonLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Responses;
using CommonLib.Dto.Requests;
using CommonLib.Dto.Responses;
using ConsoleMessengerServer.Requests;

namespace ConsoleMessengerServer.Entities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, SignUpRequestDto>().ReverseMap();
            CreateMap<UserSearchResponse, UserSearchResponseDto>().ReverseMap();
            CreateMap<int, User>().ForMember(dest => dest.Id, exp => exp.MapFrom(integ => integ.GetHashCode()));

            CreateMap<Dialog, DialogDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogRequestDto>().ForMember(dest => dest.UsersId, exp => exp.MapFrom(dial => dial.Users));
            CreateMap<CreateDialogRequestDto, Dialog>();
            CreateMap<CreateDialogResponse, CreateDialogResponseDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogResponse>().ForMember(dest => dest.DialogId, exp => exp.MapFrom(dial => dial.Id))
                                                     .ForMember(dest => dest.MessageId, exp => exp.MapFrom(dial => dial.Messages.First().Id)).ReverseMap();
            CreateMap<DeleteDialogRequestForClient, DeleteDialogRequestDto>().ReverseMap();
            CreateMap<Response, ResponseDto>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<SendMessageRequestDto, SendMessageRequest>().ReverseMap();
            CreateMap<SendMessageResponse, SendMessageResponseDto>().ReverseMap();
            CreateMap<MessagesAreReadRequestForClient, ReadMessagesRequestDto>().ReverseMap();
            CreateMap<DeleteMessageRequestForClient, DeleteMessageRequestDto>().ReverseMap();

            CreateMap<SignUpResponse, SignUpResponseDto>().ReverseMap();
            CreateMap<SignInResponse, SignInResponseDto>().ReverseMap();
        }
    }
}