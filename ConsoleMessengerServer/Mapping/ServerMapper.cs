using AutoMapper;

namespace ConsoleMessengerServer.Entities.Mapping
{
    /// <summary>
    /// Маппер серверного приложения для мапинга DTO
    /// </summary>
    public sealed class ServerMapper
    {
        /// <summary>
        /// Единственный экземпляр мапера
        /// </summary>
        private static ServerMapper _instance;

        /// <summary>
        /// Предоставляет конфигурацию для мапинга
        /// </summary>
        private Profile _profile;

        /// <summary>
        /// Приватный конструктор по умолчанию
        /// </summary>
        private ServerMapper()
        {
            _profile = new MappingProfile();
        }

        /// <summary>
        /// Метод получения единственного экземпляра мапинга
        /// </summary>
        /// <returns>Единственный экземпляр ServerMapper</returns>
        public static ServerMapper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServerMapper();
            }

            return _instance;
        }

        /// <summary>
        /// Создать мапер
        /// </summary>
        /// <returns>Маппер</returns>
        public IMapper CreateIMapper() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(_profile);
        }).CreateMapper();
    }
}