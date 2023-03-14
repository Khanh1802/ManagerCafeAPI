using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.Property(x => x.Code).IsRequired().HasMaxLength(300);
            builder.Property(x => x.CreateTime).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.TotalBill).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(200);
            builder.Property(x => x.StripeOrderId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.HasPayment).HasDefaultValue(false);
            //builder.HasOne(x => x.Staff).WithMany().HasForeignKey(x => x.StaffId).OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.ClientSetNull);

            //Nếu chưa có khai báo prop user thì sử dụng cách dưới
            //builder.HasOne<User>().WithMany().HasForeignKey(x => x.CustomerId);
            //builder.HasIndex(x => x.StaffId);
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Code);
            builder.HasIndex(x => x.Url);
            builder.HasIndex(x => x.StripeOrderId);
        }
    }
}
