using ProtoBuf;

namespace CommonLib.Dto
{
    /// <summary>
    /// Data transfer object представляет пользователя
    /// </summary>
    [ProtoContract]
    public class UserDto
    {
        /// <summary>
        /// Id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; init; }

        /// <summary>
        /// Имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; init; }

        /// <summary>
        /// Номер телефона
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Пароль
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public string Password { get; init; }

        /// <summary>
        /// Переопределение метода ToString
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Id: {Id}. PhoneNumber: {PhoneNumber}. Password: {Password}.";
        }
    }
}