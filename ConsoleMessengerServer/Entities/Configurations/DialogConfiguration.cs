using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleMessengerServer.Entities.Configurations
{
    /// <summary>
    /// Конфигурация для сущности Dialog
    /// </summary>
    public class DialogConfiguration : IEntityTypeConfiguration<Dialog>
    {
        /// <summary>
        /// Настраивает сущность Dialog
        /// </summary>
        /// <param name="builder">Строитель, который используется для конфигурации сущности</param>
        public void Configure(EntityTypeBuilder<Dialog> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasMany(d => d.Users).WithMany(acc => acc.Dialogs);

            builder.HasMany(d => d.Messages).WithOne(m => m.Dialog);
        }
    }
}