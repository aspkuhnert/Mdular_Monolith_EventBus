using BankingModule.Application;
using BankingModule.Database;
using BankingModule.Domain.IntegrationEvents;
using BankingModule.Domain.Model;
using BuildingBlocks.EventBus;
using BuildingBlocks.IntegrationEventLog;
using BuildingBlocks.IntegrationEventLog.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;
using TransferModule.Database;
using TransferModule.Application;
using TransferModule.Domain.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankingContext>(options =>
{
   options.UseSqlServer(
      builder.Configuration.GetConnectionString("ConnectionString:SqlServer:Banking"),
      sqlServerOptionsAction: sqlOptions =>
      {
         sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
         sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
      });
},
   ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
);

builder.Services.AddDbContext<IntegrationEventLogContext>(options =>
{
   options.UseSqlServer(
      builder.Configuration.GetConnectionString("ConnectionString:SqlServer:Banking"),
      sqlServerOptionsAction: sqlOptions =>
      {
         sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
         sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
      });
});

builder.Services.AddDbContext<TransferContext>(options =>
{
   options.UseSqlServer(
      builder.Configuration.GetConnectionString("ConnectionString:SqlServer:Transfer"),
      sqlServerOptionsAction: sqlOptions =>
      {
         sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
         sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
      });
},
   ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
);

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

builder.Services.AddTransient<Func<DbConnection, Assembly, IIntegrationEventLogService>>(
   services => (DbConnection connection, Assembly assembly) => new IntegrationEventLogService(connection, assembly));
builder.Services.AddTransient<IBankingIntegrationEventService, BankingIntegrationEventService>();

builder.Services.AddSingleton<IEventBus, InMemoryEventBus>(services =>
{
   var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
   var logger = services.GetRequiredService<ILogger<InMemoryEventBus>>();
   var eventBusSubcriptionsManager = services.GetRequiredService<IEventBusSubscriptionsManager>();

   return new InMemoryEventBus(
      scopeFactory,
      logger,
      eventBusSubcriptionsManager);
});

builder.Services.AddMediatR(
   typeof(Program).Assembly,
   typeof(BankingContext).Assembly,
   typeof(TransferContext).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BankingModule.Application.Behaviours.TransactionBehaviour<,>));

builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAccountManager, AccountManager>();

builder.Services.AddTransient<ITransferRepository, TransferRepository>();
builder.Services.AddTransient<ITransferManager, TransferManager>();

builder.Services.AddTransient<TransferModule.Domain.IntegrationEvents.TransferCreatedIntegrationEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<TransferModule.Domain.IntegrationEvents.TransferCreatedIntegrationEvent, TransferModule.Domain.IntegrationEvents.TransferCreatedIntegrationEventHandler>();

app.Run();
