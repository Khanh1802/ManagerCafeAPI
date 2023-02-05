using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManagerCafe.Data.Configurations
{
    public class UserTypeEntityTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserType");
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => x.Name).IsFullText();
        }
    }
}
