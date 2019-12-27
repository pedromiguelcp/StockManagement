CREATE TABLE [dbo].[Integrator]
(
	[Id_Integrator] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name_Integrator] NVARCHAR(50) NOT NULL, 
    [Phone_Integrator] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Mail_Integrator] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Address_Integrator] NVARCHAR(50) NULL DEFAULT ('Nada a registar'), 
    [Nif_Integrator] NVARCHAR(50) NULL DEFAULT ('Nada a registar')
)
