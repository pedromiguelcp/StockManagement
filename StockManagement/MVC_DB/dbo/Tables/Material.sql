CREATE TABLE [dbo].[Material]
(
	[Id_Material] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Material] NVARCHAR(50) NOT NULL, 
    [Specs_Material] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Id_Section] INT NULL DEFAULT 0 
)
