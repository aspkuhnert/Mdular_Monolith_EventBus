CREATE TABLE [IntegrationEventLog] (
    [EventId] uniqueidentifier NOT NULL,
    [EventTypeName] nvarchar(max) NOT NULL,
    [State] int NOT NULL,
    [TimesSent] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [TransactionId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_IntegrationEventLog] PRIMARY KEY ([EventId])
);
GO


