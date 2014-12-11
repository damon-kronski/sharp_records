CREATE TABLE [dbo].[ExampleItem] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [value] VARCHAR (255) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
