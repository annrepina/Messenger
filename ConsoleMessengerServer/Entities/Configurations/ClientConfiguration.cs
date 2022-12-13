using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        /// <summary>
        /// Конфигурирует модель
        /// </summary>
        /// <param name="builder">Строитель для конфигурации</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(cl => cl.Id);

            builder.HasOne(cl => cl.UserAccount).WithMany(acc => acc.Clients).HasForeignKey(cl => cl.UserAccountId);
        }
    }
}
