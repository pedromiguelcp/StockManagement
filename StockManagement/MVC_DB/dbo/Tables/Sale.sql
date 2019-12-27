CREATE TABLE [dbo].[Sale]
(
	[Id_Sale] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Id_Client] INT NOT NULL, 
    [Id_Material] NVARCHAR(50) NOT NULL, 
    [Id_Project] INT NOT NULL, 
    [Date_Sale] NVARCHAR(50) NOT NULL
)
