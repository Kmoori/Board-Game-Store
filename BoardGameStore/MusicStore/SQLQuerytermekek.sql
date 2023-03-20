CREATE TABLE [dbo].[Termekeks] (
    [Id]    INT     identity(1,1)   NOT NULL,
    [nev]   NVARCHAR (255) NULL,
    [ar]    INT            NOT NULL,
    [tipus] NVARCHAR (255) NULL,
    [kep]   NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);