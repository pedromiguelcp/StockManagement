CREATE TABLE [dbo].[Machine]
(
	[Id_Machine] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Machine] NVARCHAR(50) NOT NULL, 
    [Description_Machine] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [TotalCost_Machine] DECIMAL(8, 2) NULL DEFAULT 0.0, 
    [Id_Project] INT NOT NULL  
)
