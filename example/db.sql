CREATE TABLE [dbo].[examplemodel] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [value] VARCHAR (255) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
