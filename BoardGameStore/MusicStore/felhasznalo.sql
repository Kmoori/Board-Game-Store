CREATE TABLE [dbo].[Felhasznalok] (
    [Id]     INT     identity(1,1)     NOT NULL,
    [nev]    NVARCHAR (255) NULL,
    [jelszo] NVARCHAR (255) NULL,
    [email]  NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

