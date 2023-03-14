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
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(300);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.StripePriceId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.StripeProductId).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.OrderId);
        }
    }
}
