CREATE TABLE [dbo].[Project]
(
	[Id_Project] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Project] NVARCHAR(50) NOT NULL, 
    [Description_Project] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [TotalCost_Project] DECIMAL(8, 2) NULL DEFAULT 0.0
)
