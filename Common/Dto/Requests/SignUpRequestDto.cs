using ProtoBuf;

namespace Common.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на регистрацию пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class SignUpRequestDto
    {
        /// <summary>
        /// Номер телефона
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Пароль
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string Password { get; init; }

        /// <summary>
        /// Имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string Name { get; init; }

        /// <summary>
        /// Переопределение метода ToString
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Телефон: {PhoneNumber}. Имя: {Name}";
        }
    }
}