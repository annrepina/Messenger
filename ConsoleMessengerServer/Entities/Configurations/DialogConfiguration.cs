using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class DialogConfiguration : IEntityTypeConfiguration<Dialog>
    {
        public void Configure(EntityTypeBuilder<Dialog> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasMany(d => d.UsersDataList).WithMany(acc => acc.Dialogs);

            builder.HasMany(d => d.Messages).WithOne(m => m.Dialog);
        }
    }
}
