using ManagerCafe.Data.Configurations;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ManagerCafe.Data.Data
{
    public class ManagerCafeDbContext : DbContext
    {
        //private readonly StreamWriter _logStream = new StreamWriter("log.txt", append: true);
        public ManagerCafeDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);
            // IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();
            // var connectionString = configuration.GetConnectionString("ManagerCafe");
            // optionsBuilder.UseSqlServer(connectionString);

            //optionsBuilder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);
            //optionsBuilder.LogTo(_logStream.WriteLine, LogLevel.Debug);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ProductEntityTypeConfigurations().Configure(modelBuilder.Entity<Product>());
            new InventoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Inventory>());
            new WareHouseEntityTypeConfiguration().Configure(modelBuilder.Entity<WareHouse>());
            new InventoryTransactionEntityTypeConfiguration().Configure(modelBuilder.Entity<InventoryTransaction>());
            new UserTypeEntityTypeConfiguration().Configure(modelBuilder.Entity<UserType>());
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new OrderEntityTypeConfiguration().Configure(modelBuilder.Entity<Order>());
            new OrderDetailEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderDetail>());


            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderDetails)
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            modelBuilder.Entity<Product>()
                .HasOne<OrderDetail>(x => x.OrderDetail)
                .WithOne(x => x.Product)
                .HasForeignKey<OrderDetail>(x => x.ProductId);

        }
        //public override void Dispose()
        //{
        //    base.Dispose();
        //    _logStream.Dispose();
        //}

        //public override async ValueTask DisposeAsync()
        //{
        //    await base.DisposeAsync();
        //    await _logStream.DisposeAsync();
        //}
        #region DbSet
        public DbSet<Inventory> Invetories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
