using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class UserDataConfiguration : IEntityTypeConfiguration<UserData>
    {
        private const int MaxLengthOfPassword = 10;

        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasKey(acc => acc.Id);

            builder.HasOne(acc => acc.Person).WithOne(P => P.UserData).HasForeignKey<UserData>(acc => acc.PersonId).IsRequired();

            builder.Property(acc => acc.Password).HasMaxLength(MaxLengthOfPassword).IsRequired();

            builder.Property(acc => acc.IsOnline).IsRequired().HasDefaultValue(false);

            builder.HasMany(acc => acc.Dialogs).WithMany(d => d.UsersData);

            builder.HasMany(acc => acc.NetworkProviders).WithOne(cl => cl.UserData);

            builder.HasMany(acc => acc.SentMessages).WithOne(m => m.UserData);

            builder.HasAlternateKey(acc => acc.PersonId);
        }
    }
}
