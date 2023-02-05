using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class InvetoryEntityTypeConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");
            builder.Property(x => x.Quatity).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            // Trong table Inventory sẽ có một product hoặc nhiều product trong Inventory
            builder.HasOne(x => x.Product).WithMany(x => x.Invetories).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.WareHouse).WithMany(x => x.Invetories).HasForeignKey(x => x.WareHouseId);

            //Chỉ đánh Index Unique cho trường ProductId với điều kiện IsDeleted = 0 (Chưa xóa)
            //builder.HasIndex(x => x.ProductId).IsUnique().HasFilter("[IsDeleted] <> 1");
            //builder.HasIndex(x => x.WareHouseId).IsUnique().HasFilter("[IsDeleted] <> 1");
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.WareHouseId);
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
