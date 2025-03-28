﻿/*
Deployment script for StockManagement

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "StockManagement"
:setvar DefaultFilePrefix "StockManagement"
:setvar DefaultDataPath "C:\Users\pedro\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"
:setvar DefaultLogPath "C:\Users\pedro\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
The column [dbo].[Machine].[Id_Project] on table [dbo].[Machine] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The type for column TotalCost_Machine in table [dbo].[Machine] is currently  INT NOT NULL but is being changed to  DECIMAL (8, 2) NULL. Data loss could occur.
*/

IF EXISTS (select top 1 1 from [dbo].[Machine])
    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT

GO
PRINT N'Rename refactoring operation with key 6877c6ce-e9c2-49bc-b365-2faea0b23079 is skipped, element [dbo].[Sale].[Id] (SqlSimpleColumn) will not be renamed to Id_Sale';


GO
PRINT N'Rename refactoring operation with key 0b52f9ec-00d3-420e-8b55-03dbaf3019a7 is skipped, element [dbo].[MachineMaterial].[Id] (SqlSimpleColumn) will not be renamed to Id_MachineMaterial';


GO
PRINT N'Rename refactoring operation with key 4690f864-459b-45a2-817d-d824302c7c38 is skipped, element [dbo].[MachineMaterial].[Quantity_Material] (SqlSimpleColumn) will not be renamed to Amount_Material';


GO
PRINT N'The following operation was generated from a refactoring log file 8d995d2d-3300-4043-af9c-3ef2a0102144';

PRINT N'Rename [dbo].[Machine].[Id_Project] to TotalCost_Machine';


GO
EXECUTE sp_rename @objname = N'[dbo].[Machine].[Id_Project]', @newname = N'TotalCost_Machine', @objtype = N'COLUMN';


GO
PRINT N'Dropping unnamed constraint on [dbo].[Project]...';


GO
ALTER TABLE [dbo].[Project] DROP CONSTRAINT [DF__Project__TotalCo__656C112C];


GO
PRINT N'Altering [dbo].[Machine]...';


GO
ALTER TABLE [dbo].[Machine] ALTER COLUMN [TotalCost_Machine] DECIMAL (8, 2) NULL;


GO
ALTER TABLE [dbo].[Machine]
    ADD [Id_Project] INT NOT NULL;


GO
PRINT N'Creating [dbo].[MachineMaterial]...';


GO
CREATE TABLE [dbo].[MachineMaterial] (
    [Id_MachineMaterial] INT            IDENTITY (1, 1) NOT NULL,
    [Id_Machine]         INT            NOT NULL,
    [Id_Material]        INT            NOT NULL,
    [Amount_Material]    INT            NULL,
    [Cost_Material]      DECIMAL (8, 2) NULL,
    [Observations]       NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id_MachineMaterial] ASC)
);


GO
PRINT N'Creating [dbo].[Sale]...';


GO
CREATE TABLE [dbo].[Sale] (
    [Id_Sale]     INT           IDENTITY (1, 1) NOT NULL,
    [Id_Client]   INT           NOT NULL,
    [Id_Material] NVARCHAR (50) NOT NULL,
    [Id_Project]  INT           NOT NULL,
    [Date_Sale]   NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_Sale] ASC)
);


GO
PRINT N'Creating unnamed constraint on [dbo].[Project]...';


GO
ALTER TABLE [dbo].[Project]
    ADD DEFAULT 0.0 FOR [TotalCost_Project];


GO
PRINT N'Creating unnamed constraint on [dbo].[MachineMaterial]...';


GO
ALTER TABLE [dbo].[MachineMaterial]
    ADD DEFAULT 0 FOR [Amount_Material];


GO
PRINT N'Creating unnamed constraint on [dbo].[MachineMaterial]...';


GO
ALTER TABLE [dbo].[MachineMaterial]
    ADD DEFAULT 0.0 FOR [Cost_Material];


GO
PRINT N'Creating unnamed constraint on [dbo].[MachineMaterial]...';


GO
ALTER TABLE [dbo].[MachineMaterial]
    ADD DEFAULT ('Nada a registar') FOR [Observations];


GO
PRINT N'Creating unnamed constraint on [dbo].[Machine]...';


GO
ALTER TABLE [dbo].[Machine]
    ADD DEFAULT ('Nada a registar') FOR [Description_Machine];


GO
PRINT N'Creating unnamed constraint on [dbo].[Machine]...';


GO
ALTER TABLE [dbo].[Machine]
    ADD DEFAULT 0.0 FOR [TotalCost_Machine];


GO
-- Refactoring step to update target server with deployed transaction logs
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '6877c6ce-e9c2-49bc-b365-2faea0b23079')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('6877c6ce-e9c2-49bc-b365-2faea0b23079')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '0b52f9ec-00d3-420e-8b55-03dbaf3019a7')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('0b52f9ec-00d3-420e-8b55-03dbaf3019a7')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '4690f864-459b-45a2-817d-d824302c7c38')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('4690f864-459b-45a2-817d-d824302c7c38')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8d995d2d-3300-4043-af9c-3ef2a0102144')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8d995d2d-3300-4043-af9c-3ef2a0102144')

GO

GO
PRINT N'Update complete.';


GO
