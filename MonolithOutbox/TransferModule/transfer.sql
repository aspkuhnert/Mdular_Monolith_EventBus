CREATE TABLE [TransferLogs] (
    [Id] int NOT NULL IDENTITY,
    [FromAccount] int NOT NULL,
    [ToAccount] int NOT NULL,
    [TransferAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_TransferLogs] PRIMARY KEY ([Id])
);
GO


