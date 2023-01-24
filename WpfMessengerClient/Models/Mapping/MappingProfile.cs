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
            CreateMap<DeleteDialogRequest, DeleteDialogRequestDto>().ReverseMap();
            CreateMap<DeleteDialogRequestForClientDto, DeleteDialogRequestForClient>().ReverseMap();
            //CreateMap<DeleteDialogResponseDto, DeleteDialogResponse>().ReverseMap();
            CreateMap<ResponseDto, Response>().ReverseMap();

            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<SendMessageRequest, SendMessageRequestDto>().ReverseMap();
            CreateMap<SendMessageResponseDto, SendMessageResponse>().ReverseMap();   
            CreateMap<DeleteMessageRequest, DeleteMessageRequestDto>().ReverseMap();
            //CreateMap<DeleteMessageResponseDto, DeleteMessageResponse>().ReverseMap();
            CreateMap<DeleteMessageRequestForClientDto, DeleteMessageRequestForClient>().ReverseMap();
            CreateMap<MessagesAreReadRequest, MessagesAreReadRequestDto>().ReverseMap();
            CreateMap<MessagesAreReadRequestForClientDto, MessagesAreReadRequestForClient>().ReverseMap();

            CreateMap<ClientNetworkProvider, NetworkProviderDto>().ReverseMap();

            CreateMap<SignUpRequest, SignUpRequestDto>().ReverseMap();        
            CreateMap<SignUpRequest, User>().ReverseMap();
            CreateMap<SignUpResponse, SignUpResponseDto>().ReverseMap();

            CreateMap<SignInRequest, SignInRequestDto>().ReverseMap();
            CreateMap<SignInResponseDto, SignInResponse>().ReverseMap();

            CreateMap<UserSearchRequest, UserSearchRequestDto>().ReverseMap();
            CreateMap<UserSearchResponse, UserSearchResponseDto>().ReverseMap();

            CreateMap<SignOutRequest, SignOutRequestDto>().ReverseMap(); 
            //CreateMap<SignOutResponseDto, SignOutResponse>().ReverseMap();
        }
    }
}