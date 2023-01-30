using AutoMapper;
using Common.NetworkServices;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestProcessing.RequestHandlers;

namespace ConsoleMessengerServer.RequestProcessing
{
    /// <summary>
    /// Создает с помощью фабричного метода экземпляр конкретного обработчика запроса от клиента
    /// </summary>
    public static class RequestHandlerCreator
    {
        /// <summary>
        /// Фабричный метод создания обработчика запроса от клиента
        /// </summary>
        /// <param name="mapper">Маппер для мапинга DTO</param>
        /// <param name="connectionController">Jтвечает за соединение по сети с клиентами</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Обработчик сетевого сообщения</returns>
        public static RequestHandler FactoryMethod(IMapper mapper, IConnectionController connectionController, NetworkMessageCode code)
        {
            switch (code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    return new SignUpRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SignInRequestCode:
                    return new SignInRequestHandler(mapper, connectionController);

                case NetworkMessageCode.SearcRequestCode:
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

                case NetworkMessageCode.ReadMessagesRequestCode:
                    return new ReadMessagesRequestHandler(mapper, connectionController);

                default:
                    return null;
            }
        }
    }
}