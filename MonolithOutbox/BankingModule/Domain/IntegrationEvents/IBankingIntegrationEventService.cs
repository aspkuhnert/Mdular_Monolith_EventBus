using BuildingBlocks.EventBus.Events;

namespace BankingModule.Domain.IntegrationEvents
{
    public interface IBankingIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);

        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}