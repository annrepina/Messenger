using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const int MaxLengthOfPassword = 10;
        private const int MaxNamesLength = 50;
        private const int PhoneNumberLength = 12;

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(acc => acc.Id);

            builder.Property(p => p.Name).HasMaxLength(MaxNamesLength).IsRequired();

            builder.Property(P => P.PhoneNumber).HasMaxLength(PhoneNumberLength).IsRequired();
            builder.HasAlternateKey(p => p.PhoneNumber);

            builder.Property(acc => acc.Password).HasMaxLength(MaxLengthOfPassword).IsRequired();

            builder.Property(acc => acc.IsOnline).IsRequired().HasDefaultValue(false);

            builder.HasMany(acc => acc.Dialogs).WithMany(d => d.Users);

            builder.HasMany(acc => acc.SentMessages).WithOne(m => m.UserSender);
        }
    }
}
