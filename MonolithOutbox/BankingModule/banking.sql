CREATE TABLE [Accounts] (
    [Id] int NOT NULL IDENTITY,
    [AccountType] nvarchar(max) NOT NULL,
    [AccountBalance] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id])
);
GO


