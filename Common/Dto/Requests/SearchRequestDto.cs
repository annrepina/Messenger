using ProtoBuf;

namespace Common.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на поиск пользователя среди зарегистрированных в мессенджере
    /// </summary>
    [ProtoContract]
    public class SearchRequestDto
    {
        /// <summary>
        /// Свойство - имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; init; }

        /// <summary>
        /// Свойство - номер телефона
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
                return $"Телефон: {PhoneNumber}.";

            else if (String.IsNullOrEmpty(PhoneNumber))
                return $"Имя: {Name}.";

            return $"Имя: {Name}. Телефон: {PhoneNumber}.";
        }
    }
}