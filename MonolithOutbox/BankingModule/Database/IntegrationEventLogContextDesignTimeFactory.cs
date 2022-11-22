using BuildingBlocks.IntegrationEventLog.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BankingModule.Database
{
   public class IntegrationEventLogContextDesignTimeFactory :
     IDesignTimeDbContextFactory<IntegrationEventLogContext>
   {
      public IntegrationEventLogContext CreateDbContext(string[] args)
      {
         IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
         var connectionString = config.GetConnectionString("ConnectionString:SqlServer:Banking");

         var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogContext>()
           .UseSqlServer(
              connectionString,
              sqlOptions =>
              {
                 sqlOptions.MigrationsAssembly(typeof(BankingContext).GetTypeInfo().Assembly.GetName().Name);
              });

         return new IntegrationEventLogContext(optionsBuilder.Options);
      }
   }
}
