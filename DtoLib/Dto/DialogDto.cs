using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Interfaces;
using DtoLib.Serialization;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса Dialog
    /// </summary>
    [ProtoContract]
    public class DialogDto /*: Serializable, IDeserializableDto*/
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - аккаунт первого пользователя - участника диалога
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public UserAccountDto UserAccount1 { get; set; }

        /// <summary>
        /// Свойство - аккаунт второго пользователя - участника диалога
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public UserAccountDto UserAccount2 { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция сообщения в диалоге
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public ObservableCollection<MessageDto> Messages { get; set; }

        //public IDeserializableDto Deserialize(byte[] buffer)
        //{
        //    try
        //    {
        //        using (var stream = new MemoryStream(buffer))
        //        {
        //            var obj = Serializer.Deserialize<MessageDto>(stream);
        //            return obj;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //
        //        throw;
        //    }
        //}
    }
}
