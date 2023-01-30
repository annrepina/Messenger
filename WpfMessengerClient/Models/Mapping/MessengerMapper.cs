using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Mapping
{
    /// <summary>
    /// Маппер мессенджера, для мапинга DTO
    /// </summary>
    public sealed class MessengerMapper
    {
        /// <summary>
        /// Единственный экземпляр мапера
        /// </summary>
        private static MessengerMapper _instance;

        /// <summary>
        /// Предоставляет конфигурацию для мапинга
        /// </summary>
        private Profile _profile;

        /// <summary>
        /// Приватный конструктор по умолчанию
        /// </summary>
        private MessengerMapper() 
        {
            _profile = new MappingProfile();
        }

        /// <summary>
        /// Метод получения единственного экземпляра мапинга
        /// </summary>
        /// <returns></returns>
        public static MessengerMapper GetInstance()
        {
            if(_instance == null )
            {
                _instance = new MessengerMapper();
            }

            return _instance;
        }

        /// <summary>
        /// Создать мапер
        /// </summary>
        /// <returns></returns>
        public IMapper CreateIMapper() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(_profile);
        }).CreateMapper();
    }
}