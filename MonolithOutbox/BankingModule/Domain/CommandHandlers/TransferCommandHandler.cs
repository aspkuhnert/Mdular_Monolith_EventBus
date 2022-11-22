using BankingModule.Domain.Commands;
using BankingModule.Domain.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankingModule.Domain.CommandHandlers
{
    public class TransferCommandHandler :
      IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IBankingIntegrationEventService _integrationService;

        private readonly ILoggerFactory _logger;

        public TransferCommandHandler(IBankingIntegrationEventService service, ILoggerFactory logger)
        {
            _integrationService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<CreateTransferCommand>()
               .LogTrace("Transfer command {0} {1} - {2}", request.From, request.To, request.Amount);

            await _integrationService.AddAndSaveEventAsync(new TransferCreatedIntegrationEvent(request.From, request.To, request.Amount));

            return true;
        }
    }
}