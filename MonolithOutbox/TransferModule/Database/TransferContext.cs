using Microsoft.EntityFrameworkCore;
using TransferModule.Domain.Model;

namespace TransferModule.Database
{
    public class TransferContext :
       DbContext
    {
        public TransferContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TransferLog> TransferLogs { get; set; }
    }
}