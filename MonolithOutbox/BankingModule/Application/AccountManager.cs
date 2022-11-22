using BankingModule.Application.Model;
using BankingModule.Domain.Commands;
using BankingModule.Domain.Model;
using MediatR;

namespace BankingModule.Application
{
    public class AccountManager :
      IAccountManager
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

        public AccountManager(IAccountRepository accountRepository, IMediator mediator)
        {
            _accountRepository = accountRepository;
            _mediator = mediator;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public void Transfer(AccountTransfer accountTransfer)
        {
            var createTransferCommand = new CreateTransferCommand(
                       accountTransfer.FromAccount,
                       accountTransfer.ToAccount,
                       accountTransfer.TransferAmount
                   );

            _mediator.Send(createTransferCommand);
        }
    }
}