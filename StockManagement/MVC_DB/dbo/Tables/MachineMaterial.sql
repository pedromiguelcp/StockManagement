CREATE TABLE [dbo].[MachineMaterial]
(
	[Id_MachineMaterial] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Id_Machine] INT NOT NULL, 
    [Id_Material] INT NOT NULL, 
    [Amount_Material] INT NULL DEFAULT 0, 
    [Cost_Material] DECIMAL(8, 2) NULL DEFAULT 0.0, 
    [Observations] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [MissingMaterial] INT NULL DEFAULT 0, 
    [Priority] BIT NULL DEFAULT 0, 
    [Id_Provider] INT NOT NULL, 
    [Request] INT NULL DEFAULT 0
)
