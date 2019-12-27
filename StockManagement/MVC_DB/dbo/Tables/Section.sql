CREATE TABLE [dbo].[Section]
(
	[Id_Section] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Section] NVARCHAR(50) NOT NULL, 
    [Description_Section] NVARCHAR(50) NULL DEFAULT ('Nada a registar')
)
