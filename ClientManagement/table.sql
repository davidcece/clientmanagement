
CREATE TABLE [Clients] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Cellphone] nvarchar(12) NOT NULL,
    [InternationalPhone] nvarchar(12) NOT NULL,
    [EmailStatus] nvarchar(10) NOT NULL,
    [SmsStatus] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id]),
    CONSTRAINT [CheckEmailStatus] CHECK ([EmailStatus] IN('Active','Removed')),
    CONSTRAINT [CheckSmsStatus] CHECK ([SmsStatus] IN('Active','Removed'))
);
GO

CREATE UNIQUE INDEX [IX_Clients_Email] ON [Clients] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Clients_Email_InternationalPhone] ON [Clients] ([Email], [InternationalPhone]);
GO