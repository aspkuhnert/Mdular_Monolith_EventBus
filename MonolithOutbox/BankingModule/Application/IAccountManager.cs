using BankingModule.Application.Model;
using BankingModule.Domain.Model;

namespace BankingModule.Application
{
    public interface IAccountManager
    {
        IEnumerable<Account> GetAccounts();

        void Transfer(AccountTransfer accountTransfer);
    }
}