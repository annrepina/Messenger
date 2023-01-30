using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleMessengerServer.Entities.Configurations
{
    /// <summary>
    /// Конфигурация для сущности User
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        #region Константы

        /// <summary>
        /// Максимальная длина пароля
        /// </summary>
        private const int MaxLengthOfPassword = 10;

        /// <summary>
        /// Максимальная длина имени
        /// </summary>
        private const int MaxNamesLength = 50;

        /// <summary>
        /// Максимальная длина телефона
        /// </summary>
        private const int PhoneNumberLength = 12;

        #endregion Константы

        /// <summary>
        /// Настраивает сущность User
        /// </summary>
        /// <param name="builder">Строитель, который используется для конфигурации сущности</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(acc => acc.Id);

            builder.Property(p => p.Name).HasMaxLength(MaxNamesLength).IsRequired();

            builder.Property(P => P.PhoneNumber).HasMaxLength(PhoneNumberLength).IsRequired();
            builder.HasAlternateKey(p => p.PhoneNumber);

            builder.Property(acc => acc.Password).HasMaxLength(MaxLengthOfPassword).IsRequired();

            builder.HasMany(acc => acc.Dialogs).WithMany(d => d.Users);

            builder.HasMany(acc => acc.SentMessages).WithOne(m => m.UserSender);
        }
    }
}