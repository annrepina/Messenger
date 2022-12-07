using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto
{
    public class DialogDto
    {
        public int Id { get; set; }

        public UserAccountDto UserAccount1 { get; set; }
        //{
        //    get => _userAccount1;

        //    set
        //    {
        //        if (value != null)
        //        {
        //            _userAccount1 = value;
        //            OnPropertyChanged(nameof(UserAccount1));
        //        }

        //    }
        //}

        public UserAccountDto UserAccount2 { get; set; }
        //{
        //    get => _userAccount2;

        //    set
        //    {
        //        if (value != null)
        //        {
        //            _userAccount2 = value;
        //            OnPropertyChanged(nameof(UserAccount2));
        //        }
        //    }
        //}

        public ObservableCollection<MessageDto> Messages { get; set; }
    }
}
