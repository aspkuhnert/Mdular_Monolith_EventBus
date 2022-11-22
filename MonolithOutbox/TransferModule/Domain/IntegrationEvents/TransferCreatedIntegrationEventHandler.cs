using BuildingBlocks.EventBus.Events;
using TransferModule.Domain.Model;

namespace TransferModule.Domain.IntegrationEvents
{
    public class TransferCreatedIntegrationEventHandler :
       IIntegrationEventHandler<TransferCreatedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        public TransferCreatedIntegrationEventHandler(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public Task Handle(TransferCreatedIntegrationEvent @event)
        {
            _transferRepository.Add(new TransferLog()
            {
                FromAccount = @event.From,
                ToAccount = @event.To,
                TransferAmount = @event.Amount
            });

            return Task.CompletedTask;
        }
    }
}