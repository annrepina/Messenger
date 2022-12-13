using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Text).IsRequired();

            builder.HasOne(m => m.UserAccount).WithMany(acc => acc.Messages).HasForeignKey(m => m.UserAccountId).IsRequired();

            builder.HasOne(m => m.Dialog).WithMany(d => d.Messages).HasForeignKey(m => m.DialogId).IsRequired();

            builder.Property(m => m.IsRead).IsRequired().HasDefaultValue(false);

            builder.Property(m => m.DateTime).ValueGeneratedOnAdd();
        }
    }
}
