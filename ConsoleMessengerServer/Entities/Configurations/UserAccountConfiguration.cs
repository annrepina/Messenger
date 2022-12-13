using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        private const int MaxLengthOfPassword = 10;

        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(acc => acc.Id);

            builder.HasOne(acc => acc.Person).WithOne(P => P.UserAccount).HasForeignKey<UserAccount>(acc => acc.PersonId).IsRequired();

            builder.Property(acc => acc.Password).HasMaxLength(MaxLengthOfPassword).IsRequired();

            builder.Property(acc => acc.IsOnline).IsRequired().HasDefaultValue(false);

            builder.HasMany(acc => acc.Dialogs).WithMany(d => d.UserAccounts);

            builder.HasMany(acc => acc.Clients).WithOne(cl => cl.UserAccount);

            builder.HasMany(acc => acc.Messages).WithOne(m => m.UserAccount);

            builder.HasAlternateKey(acc => acc.PersonId);

            //builder.HasOne()
        }
    }
}
