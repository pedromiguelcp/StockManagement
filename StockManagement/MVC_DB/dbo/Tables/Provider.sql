CREATE TABLE [dbo].[Provider]
(
	[Id_Provider] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Provider] NVARCHAR(50) NOT NULL, 
    [Phone_Provider] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Mail_Provider] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Address_Provider] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Nif_Provider] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Language_Provider] INT NOT NULL
)
