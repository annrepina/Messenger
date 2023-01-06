using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System;
using DtoLib.Interfaces;
using ProtoBuf;
using DtoLib.NetworkServices;
using DtoLib.Serialization;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса UserData
    /// </summary>
    [ProtoContract]
    public class UserDataDto 
    {
        /// <summary>
        /// Свойство - идентификатор
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - объект класса, представляющего человека
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public PersonDto Person { get; set; }

        /// <summary>
        /// Свойство - пароль
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string Password { get; set; }

        /// <summary>
        /// Свойство - онлайн ли пользователь в текущий момент
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public bool IsOnline { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция диалогов у пользователя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(5)]
        public ObservableCollection<DialogDto> Dialogs { get; set; }

        #region Debug

        /// <summary>
        /// Переопределение метода ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Id: {Id}. PhoneNumber: {Person.PhoneNumber}. Password: {Password}. IsOnline: {IsOnline}. DialogsNumber: {Dialogs.Count}";
        }

        #endregion Debug
    }
}