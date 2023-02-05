using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class InventoryTransactionEntityTypeConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.ToTable("InventoryTransaction");
            builder.Property(x => x.Type).IsRequired();
            builder.HasOne(x => x.Inventory).WithMany(x => x.InventoryTransactions).HasForeignKey(x => x.InventoryId);
            builder.HasIndex(x => x.InventoryId);
        }
    }
}
