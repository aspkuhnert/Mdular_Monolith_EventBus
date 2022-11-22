using BuildingBlocks.EventBus.Events;
using BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingBlocks.EventBus
{
   public class InMemoryEventBus :
     IEventBus,
     IDisposable
   {
      private readonly IServiceScopeFactory _serviceScopeFactory;
      private readonly ILogger<InMemoryEventBus> _logger;
      private readonly IEventBusSubscriptionsManager _subsManager;

      public InMemoryEventBus(IServiceScopeFactory serviceScopeFactory, ILogger<InMemoryEventBus> logger, IEventBusSubscriptionsManager subsManager)
      {
         _serviceScopeFactory = serviceScopeFactory;
         _logger = logger;
         _subsManager = subsManager;
      }

      public void Dispose()
      {
         _subsManager.Clear();
      }

      public async void Publish(IntegrationEvent @event)
      {
         var eventName = @event.GetType().Name;

         if (_subsManager.HasSubscriptionsForEvent(eventName))
         {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
               var subscriptions = _subsManager.GetHandlersForEvent(eventName);

               foreach (var subscription in subscriptions)
               {
                  try
                  {
                     var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                     var eventType = _subsManager.GetEventTypeByName(eventName);
                     var message = JsonSerializer.Serialize(@event);
                     var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                     var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                     await Task.Yield();
                     await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                  }
                  catch (Exception ex)
                  {
                     _logger.LogWarning("No registered event handler for event: {EventName}", eventName);
                     continue;
                  }
               }
            }
         }
         else
         {
            _logger.LogWarning("No subscription for event: {EventName}", eventName);
         }
      }

      public void Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
          where TIntegrationEvent : IntegrationEvent
          where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
      {
         var eventName = _subsManager.GetEventKey<TIntegrationEvent>();
         _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TIntegrationEventHandler).GetGenericTypeName());

         _subsManager.AddSubscription<TIntegrationEvent, TIntegrationEventHandler>();
      }
   }
}
