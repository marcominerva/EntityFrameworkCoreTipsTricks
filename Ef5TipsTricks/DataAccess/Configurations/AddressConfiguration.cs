using Ef5TipsTricks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ef5TipsTricks.DataAccess.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Type).HasConversion<string>();
            builder.Property(x => x.Street).HasMaxLength(50).IsRequired();
            builder.Property(x => x.City).HasMaxLength(30).IsRequired();

            builder.HasOne(x => x.Person).WithMany(x => x.Addresses).HasForeignKey(x => x.PersonId);
        }
    }
}
