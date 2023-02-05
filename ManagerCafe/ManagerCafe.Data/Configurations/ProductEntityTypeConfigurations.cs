using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class ProductEntityTypeConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            //builder.HasKey(x => x.Id);

            //builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
            builder.Property(x => x.PriceBuy).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.PriceSell).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            //Tìm kiếm theo tên như like nhưng nhanh hơn
            builder.HasIndex(x => x.Name).IsFullText();
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
