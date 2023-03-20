CREATE TABLE [dbo].[Carts] (
    [CartId]   INT IDENTITY (1, 1) NOT NULL,
    [TermekId] INT NOT NULL,
	[Mennyiseg] INT NOT NULL,
    CONSTRAINT [PK_dbo.Carts] PRIMARY KEY CLUSTERED ([CartId] ASC),
    CONSTRAINT [FK_dbo.Carts_dbo.Termekek_Id] FOREIGN KEY ([TermekId]) REFERENCES [dbo].[Termekek] ([Id]) ON DELETE CASCADE
);

