using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DtoLib.Dto
{
    public class PersonDto : IDataTransferObject
    {
        public string Name { get; set; }

        public string? Surname { get; set; }

        public string PhoneNumber { get; set; }
    }
}
