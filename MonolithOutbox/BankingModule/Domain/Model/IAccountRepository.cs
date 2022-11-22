using BuildingBlocks.Domain.Seedwork;

namespace BankingModule.Domain.Model
{
    public interface IAccountRepository
    {
        IUnitOfWork UnitOfWork { get; }

        IEnumerable<Account> GetAccounts();

        Account Add(Account account);
    }
}