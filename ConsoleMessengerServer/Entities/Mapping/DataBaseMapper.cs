using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Mapping
{
    public sealed class DataBaseMapper
    {
        private static DataBaseMapper _instance;

        private Profile _profile;

        private DataBaseMapper()
        {
            _profile = new MappingProfile();
        }

        public static DataBaseMapper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataBaseMapper();
            }

            return _instance;
        }

        public IMapper CreateIMapper() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(_profile);
        }).CreateMapper();

      
    }
}
