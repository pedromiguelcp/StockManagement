CREATE TABLE [dbo].[StockMaterial]
(
	[Id_StockMaterial] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Id_Material] INT NOT NULL, 
    [Id_Provider] INT NOT NULL, 
    [Id_Section] INT NOT NULL, 
    [Amount_Material] INT NULL DEFAULT 0, 
    [Observations] NVARCHAR(50) NULL DEFAULT ('Nada a registar')
)
