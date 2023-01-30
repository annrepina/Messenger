using ProtoBuf;

namespace Common.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на выход пользователя из мессенджера
    /// </summary>
    [ProtoContract]
    public class SignOutRequestDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Id пользователя: {UserId}";
        }
    }
}