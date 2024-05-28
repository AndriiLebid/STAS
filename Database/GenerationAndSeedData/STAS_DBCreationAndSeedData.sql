/*
NAME: Andrii Lebid
DATE: 05/27/2024
PURPOSE: Advatek System
Description: Simple Time And Attendance System /Database Creation Script
Version: 1.2
*/

USE [STAS-A];
GO 

-- DELETE Tables

IF OBJECT_ID('[STAS-A].dbo.RawScan', 'U') IS NOT NULL
DROP TABLE [STAS-A].dbo.RawScan;

IF OBJECT_ID('[STAS-A].dbo.Employee', 'U') IS NOT NULL
DROP TABLE [STAS-A].dbo.Employee;

IF OBJECT_ID('[STAS-A].dbo.EmployeeType', 'U') IS NOT NULL
DROP TABLE [STAS-A].dbo.EmployeeType;

IF OBJECT_ID('[STAS-A].dbo.ScanType', 'U') IS NOT NULL
DROP TABLE [STAS-A].dbo.ScanType;

IF OBJECT_ID('[STAS-A].dbo.User', 'U') IS NOT NULL
DROP TABLE [STAS-A].dbo.[User];


-- Create table ScanType


IF OBJECT_ID('[STAS-A].dbo.ScanType') IS NULL
	BEGIN 
	CREATE TABLE [ScanType] (
		TypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		TypeName NVARCHAR(50) NOT NULL
	)
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'ScanTable tabel has been created'
		PRINT '------------------------------------------------------------------------------------------';
	END
ELSE
	BEGIN
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'ScanTable tabel exist';
		PRINT '------------------------------------------------------------------------------------------';
END

GO

-- Create table EmployeeType



IF OBJECT_ID('[STAS-A].dbo.EmployeeType') IS NULL
	BEGIN 
	CREATE TABLE [EmployeeType] (
		TypeEmployeeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		TypeEmployeeName NVARCHAR(50) NOT NULL
	)
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'EmployeeType tabel has been created'
		PRINT '------------------------------------------------------------------------------------------';
	END
ELSE
	BEGIN
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'EmployeeType tabel exist';
		PRINT '------------------------------------------------------------------------------------------';
END

GO


-- Create table User


 
IF OBJECT_ID('EpicSolutions.dbo.User') IS NULL
	BEGIN
	CREATE TABLE [User] (
		UserId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Password] CHAR(64) NOT NULL,
		PasswordSalt BINARY(16) NOT NULL,
		UserName NVARCHAR(30) NOT NULL,
		CONSTRAINT UQ_User_UserName UNIQUE (UserName)
		)
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'User table has been created'
		PRINT '------------------------------------------------------------------------------------------';
	END
ELSE
	BEGIN
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'User table exist';
		PRINT '------------------------------------------------------------------------------------------';
END

GO





-- Create table Employee


 
IF OBJECT_ID('EpicSolutions.dbo.Employee') IS NULL
	BEGIN
	CREATE TABLE [Employee] (
		EmployeeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		EmployeeCardNumber NVARCHAR(4) NOT NULL,
		FirstName NVARCHAR(30) NOT NULL,
		MiddleInitial NVARCHAR(1) NULL,
		LastName NVARCHAR(30) NOT NULL,
		TypeEmployeeId INT NULL,
		RecordVersion ROWVERSION NOT NULL,
		CONSTRAINT FK_Employee_TypeEmployee FOREIGN KEY (TypeEmployeeId) REFERENCES EmployeeType(TypeEmployeeId)
		)
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'Employee table has been created'
		PRINT '------------------------------------------------------------------------------------------';
	END
ELSE
	BEGIN
		PRINT '------------------------------------------------------------------------------------------';
		PRINT 'Employee table exist';
		PRINT '------------------------------------------------------------------------------------------';
END

GO

-- Create table RawScan


 
IF OBJECT_ID('EpicSolutions.dbo.RawScan') IS NULL
    BEGIN 
        CREATE TABLE [RawScan] (
            RawScanID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
			EmployeeId INT NOT NULL,
			ScanDate DateTime2 NOT NULL,
			ScanType INT NOT NULL,
			RecordVersion ROWVERSION NOT NULL,
			CONSTRAINT FK_Employee_RawScan FOREIGN KEY (ScanType) REFERENCES [Employee](EmployeeId)
        )
        PRINT '------------------------------------------------------------------------------------------';
        PRINT 'RawScan table has been created';
        PRINT '------------------------------------------------------------------------------------------';
    END
ELSE
    BEGIN
        PRINT '------------------------------------------------------------------------------------------';
        PRINT 'RawScan table exists';
        PRINT '------------------------------------------------------------------------------------------';
    END

GO



--SEED TABLES
-----------------------------------------------------------------------------------------------

 -- Add Roles

SET IDENTITY_INSERT [dbo].[EmployeeType] ON 
GO
 
INSERT INTO [EmployeeType] (TypeEmployeeId, TypeEmployeeName)
VALUES 
	(1, 'Administrator'),
	(2, 'User')
 

SET IDENTITY_INSERT [dbo].[EmployeeType] OFF 
GO

SET IDENTITY_INSERT [dbo].[ScanType] ON

INSERT INTO [ScanType] (TypeId, TypeName)
VALUES 
    (1, 'IN'),
    (2, 'OUT');

SET IDENTITY_INSERT [dbo].[ScanType] OFF


SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [Password], [PasswordSalt], [UserName]) VALUES (1, N'd2a232d95451c7d6a4264ab1fec586560fb8cc6ff59f7ea542260293b1fe5395', 0xDE12A6FD00D1ADDDC5986C978BB1CDA8, N'Administrator')
INSERT [dbo].[User] ([UserId], [Password], [PasswordSalt], [UserName]) VALUES (2, N'd2a232d95451c7d6a4264ab1fec586560fb8cc6ff59f7ea542260293b1fe5395', 0xDE12A6FD00D1ADDDC5986C978BB1CDA8, N'Admin')

SET IDENTITY_INSERT [dbo].[User] OFF
GO


SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (1,'1234', N'Quon', N'B', N'Rollins',1)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (2,'4321', N'Paula', N'A', N'King',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (3,'1214', N'Channing', N'B', N'Livingston',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (4,'1224', N'Piper', N'B', N'Trevino',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (5,'1234', N'Jonh', N'L', N'Delaney',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (6,'1244', N'Jesse', N'E', N'Holcomb',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (7,'1254', N'Hamilton', N'V', N'Rice',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (8,'1264', N'Jonah', N'L', N'Guzman',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (9,'1274', N'Mira', N'S', N'Abbott',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (10,'1284', N'Ayanna', N'P', N'Mays',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (11,'1294', N'Graham', N'G', N'Sargent',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (12,'1204', N'Giacomo', N'J', N'Greer',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (13,'1334', N'Tashya', N'R', N'Conley',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (14,'1434', N'Cameran', N'O', N'Alvarez',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (15,'1534', N'Zelda', N'P', N'Ortega',2)
INSERT [dbo].[Employee] ([EmployeeId], EmployeeCardNumber, [FirstName], [MiddleInitial], [LastName], TypeEmployeeId) VALUES (16,'1634', N'Carly', N'C', N'Humphrey',2)

SET IDENTITY_INSERT [dbo].[Employee] OFF
GO

SET IDENTITY_INSERT [dbo].[RawScan] ON 

INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (1, 1, '2023-06-01 10:25:00', 1);
INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (2, 1, '2023-06-01 11:25:00', 1);
INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (3, 2, '2023-06-01 10:27:00', 1);
INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (4, 2, '2023-06-01 11:27:00', 1);
INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (5, 2, '2023-06-01 12:25:00', 1);
INSERT [dbo].[RawScan] (RawScanID, EmployeeId, ScanDate, ScanType) VALUES (6, 2, '2023-06-01 12:25:00', 1);


SET IDENTITY_INSERT [dbo].[RawScan] OFF
GO