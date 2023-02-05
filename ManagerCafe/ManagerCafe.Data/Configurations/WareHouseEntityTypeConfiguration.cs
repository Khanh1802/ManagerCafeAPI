using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ManagerCafe.Data.Configurations
{
    public class WareHouseEntityTypeConfiguration : IEntityTypeConfiguration<WareHouse>
    {
        public void Configure(EntityTypeBuilder<WareHouse> builder)
        {
            builder.ToTable("WareHouse");
            builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.HasIndex(x => x.Name).IsFullText();
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
