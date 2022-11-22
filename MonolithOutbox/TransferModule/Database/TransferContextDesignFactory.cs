using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TransferModule.Database
{
   public class TransferContextDesignFactory :
     IDesignTimeDbContextFactory<TransferContext>
   {
      public TransferContext CreateDbContext(string[] args)
      {
         IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
         var connectionString = config.GetConnectionString("ConnectionString:SqlServer:Transfer");


         var optionsBuilder = new DbContextOptionsBuilder<TransferContext>()
            .UseSqlServer(
               connectionString,
               sqlOptions =>
               {
                  sqlOptions.MigrationsAssembly(typeof(TransferContext).GetTypeInfo().Assembly.GetName().Name);
               });

         return new TransferContext(optionsBuilder.Options);
      }
   }
}
