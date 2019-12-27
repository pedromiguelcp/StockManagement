CREATE TABLE [dbo].[ProvideMaterials]
(
	[Id_ProvideMaterials] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Id_Provider] INT NOT NULL, 
    [Id_Material] INT NOT NULL, 
    [Unit_Cost] DECIMAL(6, 2) NOT NULL, 
    [ExpirationDate] NVARCHAR(50) NOT NULL, 
    [QuotationPath] NVARCHAR(400) NOT NULL
)
