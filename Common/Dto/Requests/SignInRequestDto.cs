using ProtoBuf;

namespace Common.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на вход в мессенджер
    /// </summary>
    [ProtoContract]
    public class SignInRequestDto
    {
        /// <summary>
        /// Свойство - номер телефона
        /// </summary>
        [ProtoMember(1)]
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Свойство - пароль
        /// </summary>
        [ProtoMember(2)]
        public string Password { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Телефон: {PhoneNumber}.";
        }
    }
}