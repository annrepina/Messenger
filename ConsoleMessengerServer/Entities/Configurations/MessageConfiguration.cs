using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleMessengerServer.Entities.Configurations
{
    /// <summary>
    /// Конфигурация для сущности Message
    /// </summary>
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        /// <summary>
        /// Настраивает сущность Message
        /// </summary>
        /// <param name="builder">Строитель, который используется для конфигурации сущности</param>

        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Text).IsRequired();

            builder.HasOne(m => m.UserSender).WithMany(acc => acc.SentMessages).HasForeignKey(m => m.UserSenderId).IsRequired();

            builder.HasOne(m => m.Dialog).WithMany(d => d.Messages).HasForeignKey(m => m.DialogId).IsRequired();

            builder.Property(m => m.IsRead).IsRequired().HasDefaultValue(false);

            builder.Property(m => m.DateTime).IsRequired();
        }
    }
}