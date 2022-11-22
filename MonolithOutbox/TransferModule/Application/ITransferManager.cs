using TransferModule.Domain.Model;

namespace TransferModule.Application
{
    public interface ITransferManager
    {
        IEnumerable<TransferLog> GetTransferLogs();
    }
}