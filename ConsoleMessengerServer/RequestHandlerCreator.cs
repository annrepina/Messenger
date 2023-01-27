using AutoMapper;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestHandlers;
using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer
{
    public static class RequestHandlerCreator
    {
        public static RequestHandler FactoryMethod(IMapper mapper, IConnectionController connectionController, NetworkMessageCode code)
        {
            switch (code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    return new SignUpRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SignInRequestCode:
                    return new SignInRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SearchUserRequestCode:
                    return new SearchUserRequestHandler(mapper, connectionController);

                case NetworkMessageCode.CreateDialogRequestCode:
                    return new CreateDialogRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SendMessageRequestCode:
                    return new SendMessageRequestHandler(mapper, connectionController);

                case NetworkMessageCode.DeleteMessageRequestCode:
                    return new DeleteMessageRequestHandler(mapper, connectionController);

                case NetworkMessageCode.DeleteDialogRequestCode:
                    return new DeleteDialogRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SignOutRequestCode:
                    return new SignOutRequestHandler(mapper, connectionController);

                case NetworkMessageCode.MessagesAreReadRequestCode:
                    return new ReadMessagesRequestHandler(mapper, connectionController);

                default:
                    return null;
            }
        }
    }
}
