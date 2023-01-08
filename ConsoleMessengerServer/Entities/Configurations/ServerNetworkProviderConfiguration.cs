using DtoLib.NetworkServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class ServerNetworkProviderConfiguration : IEntityTypeConfiguration<ServerNetworkProviderEntity>
    {
        /// <summary>
        /// Конфигурирует модель
        /// </summary>
        /// <param name="builder">Строитель для конфигурации</param>
        public void Configure(EntityTypeBuilder<ServerNetworkProviderEntity> builder)
        {
            builder.ToTable(nameof(NetworkProvider) + 's');

            builder.HasKey(cl => cl.Id);

            builder.HasOne(cl => cl.UserData).WithMany(acc => acc.NetworkProviders).HasForeignKey(cl => cl.UserDataId);
        }
    }
}
