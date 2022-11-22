using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankingModule.Database
{
   public class BankingContextDesignFactory :
     IDesignTimeDbContextFactory<BankingContext>
   {
      public BankingContext CreateDbContext(string[] args)
      {
         IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
         var connectionString = config.GetConnectionString("ConnectionString:SqlServer:Banking");


         var optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
            .UseSqlServer(
               connectionString,
               sqlOptions =>
               {
                  sqlOptions.MigrationsAssembly(typeof(BankingContext).GetTypeInfo().Assembly.GetName().Name);
               });

         return new BankingContext(optionsBuilder.Options);
      }
   }
}
