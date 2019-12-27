CREATE TABLE [dbo].[Supply] (
    [Id_Supply]        INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Id_Provider]      INT            NOT NULL,
    [Id_Material]      INT            NOT NULL,
    [Id_Section]       INT            NOT NULL,
    [Amount_Material]  INT            NOT NULL,
    [TotalPrice_Supply] DECIMAL (6, 2)  NOT NULL,
    [Date_Supply]      NVARCHAR(50)    NOT NULL, 
    [Request] INT NOT NULL, 
    [TotalPrice_Material] DECIMAL(6, 2) NOT NULL, 
    [SupplyOrder] INT NOT NULL
);

