﻿using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System;
using DtoLib.Interfaces;
using ProtoBuf;
using DtoLib.NetworkServices;
using DtoLib.Serialization;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса UserAccount
    /// </summary>
    [ProtoContract]
    public class UserAccountDto 
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - объект клааса Person
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
        /// Свойство - является ли онлайн аккаунт
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public bool IsOnline { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция диалогов у аккаунта
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(5)]
        public ObservableCollection<DialogDto> Dialogs { get; set; }

        /// <summary>
        /// Свойство - список клиентов у аккаунта, те клиенты на которых открыта учетка аккаунта
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(6)]
        public List<ClientDto> Clients { get; set; }



        #region Debug

        public override string ToString()
        {
            return $"Id: {Id}. PhoneNumber: {Person.PhoneNumber}. Password: {Password}. IsOnline: {IsOnline}. DialogsNumber: {Dialogs.Count}";
        }

        #endregion Debug

    }
}