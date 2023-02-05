using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");
            builder.Property(x => x.Quaity).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(300);
            builder.Property(x => x.WarehouseName).IsRequired().HasMaxLength(300);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.WareHouse).WithMany().HasForeignKey(x => x.WareHouseId);

            builder.HasIndex(x => x.OrderId);
        }
    }
}
