using AutoMapper;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources.Extensions;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.Services;

namespace WpfMessengerClient.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, int>().ConvertUsing(source => source.Id);

            CreateMap<Dialog, DialogDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogRequestDto>().ForMember(dest => dest.UsersId, exp => exp.MapFrom(dial => dial.Users)).ReverseMap();
            CreateMap<CreateDialogResponse, CreateDialogResponseDto>().ReverseMap();
            CreateMap<Dialog, CreateDialogResponse>().ForMember(dest => dest.DialogId, exp => exp.MapFrom(dial => dial.Id))
                                                     .ForMember(dest => dest.MessageId, exp => exp.MapFrom(dial => dial.Messages.First().Id)).ReverseMap();
            CreateMap<ExtendedDeleteDialogRequest, DeleteDialogRequestDto>().ReverseMap();
            CreateMap<DeleteDialogRequestForClientDto, DeleteDialogRequest>().ReverseMap();
            CreateMap<ResponseDto, Response>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<SendMessageRequest, SendMessageRequestDto>().ReverseMap();
            CreateMap<SendMessageResponseDto, SendMessageResponse>().ReverseMap();   
            CreateMap<ExtendedDeleteMessageRequest, DeleteMessageRequestDto>().ReverseMap();
            CreateMap<DeleteMessageRequestForClientDto, DeleteMessageRequest>().ReverseMap();
            CreateMap<ExtendedReadMessagesRequest, MessagesAreReadRequestDto>().ReverseMap();
            CreateMap<MessagesAreReadRequestForClientDto, ReadMessagesRequest>().ReverseMap();

            CreateMap<SignUpRequest, SignUpRequestDto>().ReverseMap();        
            CreateMap<SignUpRequest, User>().ReverseMap();
            CreateMap<SignUpResponse, SignUpResponseDto>().ReverseMap();

            CreateMap<SignInRequest, SignInRequestDto>().ReverseMap();
            CreateMap<SignInResponseDto, SignInResponse>().ReverseMap();

            CreateMap<SearchRequest, SearchRequestDto>().ReverseMap();
            CreateMap<SearchResponse, UserSearchResponseDto>().ReverseMap();

            CreateMap<SignOutRequest, SignOutRequestDto>().ReverseMap(); 
        }
    }
}