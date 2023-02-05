using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ManagerCafe.Data.Data
{
    public class ManagerCafeDbContextFactory : IDesignTimeDbContextFactory<ManagerCafeDbContext>
    {
        public ManagerCafeDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //var connectionString = configuration.GetConnectionString("ManagerCafe");
            //var optionsBuilder = new DbContextOptionsBuilder<ManagerCafeDbContext>();
            //optionsBuilder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);
            //return new ManagerCafeDbContext(optionsBuilder.Options);

            var connectionString = configuration.GetConnectionString("ManagerCafe");
            var optionsBuilder = new DbContextOptionsBuilder<ManagerCafeDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ManagerCafeDbContext(optionsBuilder.Options);
        }
    }
}
