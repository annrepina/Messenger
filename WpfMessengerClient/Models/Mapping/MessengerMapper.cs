using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Mapping
{
    public sealed class MessengerMapper
    {
        private static MessengerMapper _instance;

        private Profile _profile;

        private MessengerMapper() 
        {
            _profile = new MappingProfile();
        }

        public static MessengerMapper GetInstance()
        {
            if(_instance == null )
            {
                _instance = new MessengerMapper();
            }

            return _instance;
        }

        public IMapper CreateIMapper() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(_profile);
        }).CreateMapper();

    }
}
