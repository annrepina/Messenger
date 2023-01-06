using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        private const int MaxNamesLength = 50;
        private const int PhoneNumberLength = 12;

        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(MaxNamesLength).IsRequired();
            builder.Property(p => p.Surname).HasMaxLength(MaxNamesLength);
            builder.Property(P => P.PhoneNumber).HasMaxLength(PhoneNumberLength).IsRequired();
            builder.HasAlternateKey(p => p.PhoneNumber);
        }
    }
}