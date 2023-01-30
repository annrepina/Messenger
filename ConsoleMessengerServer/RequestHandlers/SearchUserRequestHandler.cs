﻿using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;
using CommonLib.Dto.Requests;
using CommonLib.Dto.Responses;
using CommonLib.NetworkServices;
using CommonLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.RequestHandlers
{
    public class SearchUserRequestHandler : RequestHandler
    {
        public SearchUserRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            UserSearchResponse errorResponse = new UserSearchResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<UserSearchResponse, UserSearchResponseDto>(networkProvider, errorResponse, NetworkMessageCode.SearchResponseCode);
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SearchRequestDto searchRequestDto = SerializationHelper.Deserialize<SearchRequestDto>(networkMessage.Data);

            List<User> usersList = dbService.FindListOfUsers(searchRequestDto);

            UserSearchResponse response = CreateUserSearchResponse(usersList);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out UserSearchResponseDto responseDto, NetworkMessageCode.SearchResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<UserSearchResponse, UserSearchResponseDto>.Convert(response, NetworkMessageCode.SearchResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SearchResponseCode, searchRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        private UserSearchResponse CreateUserSearchResponse(List<User> usersList)
        {
            if (usersList.Count > 0)
                return new UserSearchResponse(usersList, NetworkResponseStatus.Successful);

            return new UserSearchResponse(NetworkResponseStatus.Failed);
        }
    }
}
