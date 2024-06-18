/*
NAME: Andrii Lebid
DATE: 05/27/2024
PURPOSE: Advatek System
Description: Simple Time And Attendance System / Stored Procedures
Version: 1.3
*/


USE [STAS-A];
GO

-- User Login Procedures
---------------------------------

-- Serche user login and Password

CREATE OR ALTER PROC [dbo].[spLogin]
@UserName AS NVARCHAR(30),
@Password AS NVARCHAR(64)
AS
BEGIN
	BEGIN TRY
		SELECT * FROM [User] INNER JOIN [Role] ON [User].UserRole = [Role].RoleId 
		WHERE [User].UserName = @UserName AND [User].[Password] = @Password
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO


-- Get password salt

CREATE OR ALTER PROC [dbo].[spGetPasswordSalt]
@UserName AS NVARCHAR(30)
AS
BEGIN
	BEGIN TRY
		SELECT PasswordSalt FROM [User] WHERE UserName = @UserName
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get password salt

CREATE OR ALTER PROC [dbo].[spGetAllUser]
AS
BEGIN
	BEGIN TRY
		SELECT * FROM [User]
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Create new user

CREATE OR ALTER PROC [dbo].[spAddUser]
	@UserId AS INT OUTPUT,
	@UserName AS NVARCHAR(30),
	@Password AS CHAR(64),
	@Salt AS BINARY(16),
	@UserTypeId AS INT
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		
		 INSERT INTO [dbo].[User] (
            [UserName],
            [UserRole],
            [Password],
            [PasswordSalt]	
        )
        VALUES (
            @UserName,
            @UserTypeId,
            @Password,
            @Salt
        );

		SET @UserId = SCOPE_IDENTITY();
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- Edit user

CREATE OR ALTER PROC [dbo].[spEditUser]
	@UserId AS INT,
	@UserName AS NVARCHAR(30),
	@UserTypeId AS INT
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		
		 UPDATE [User]
			SET
				UserName = @UserName,
				UserRole = @UserTypeId
			WHERE 
				UserId = @UserId;
		SET @UserId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO


-- Edit user password

CREATE OR ALTER PROC [dbo].[spEditUserPassword]
	@UserId AS INT,
	@Salt AS BINARY(16),
	@Password AS CHAR(64)
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		
		 UPDATE [User]
			SET
				PasswordSalt = @Salt,
				[Password] = @Password
			WHERE 
				UserId = @UserId;
		SET @UserId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- Get password salt

CREATE OR ALTER PROC [dbo].[spGetUserById]
@UserId AS INT
AS
BEGIN
	BEGIN TRY
		SELECT * FROM [User] WHERE [User].UserId = @UserId
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get Role

CREATE OR ALTER PROC [dbo].[spGetRole]
AS
BEGIN
	BEGIN TRY
		SELECT * FROM [Role]
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Delete user record

CREATE OR ALTER PROC [dbo].[spDeleteUser]
@UserId AS INT
AS
BEGIN
	BEGIN TRY
		DELETE FROM [User] WHERE UserId = @UserId;
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO



-- Employee procedures
-----------------------------------------

-- Get all Employee

CREATE OR ALTER PROC [dbo].[spGetAllEmployees]
AS
BEGIN
	BEGIN TRY
		SELECT * FROM Employee
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO


--Get employee be id

CREATE OR ALTER PROC [dbo].[spSearchEmployeesById]
@EmployeeId AS INT
AS
BEGIN
	BEGIN TRY
		SELECT * FROM Employee WHERE EmployeeId = @EmployeeId
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

--Get employee be card number

CREATE OR ALTER PROC [dbo].[spSearchEmployeesByNumber]
@EmployeeCardNumber AS INT
AS
BEGIN
	BEGIN TRY
		SELECT * FROM Employee WHERE EmployeeCardNumber = @EmployeeCardNumber
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get employee id by card number

CREATE OR ALTER PROC [dbo].spGetEmployeeIdByCardNumber
@CardNumber AS INT
AS
BEGIN
	BEGIN TRY
		SELECT EmployeeId FROM Employee WHERE EmployeeCardNumber = @CardNumber
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Create new employee

CREATE OR ALTER PROC [dbo].[spAddEmployee]
	@EmployeeId AS INT OUTPUT,
	@EmployeeCardNumber AS NVARCHAR(4),
	@FirstName AS NVARCHAR(30),
	@MiddleInitial AS NVARCHAR(1),
	@LastName AS NVARCHAR(30),
	@EmployeeTypeId AS INT
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		
		 INSERT INTO [dbo].[Employee] (
			[EmployeeCardNumber],
            [FirstName],
            [MiddleInitial],
            [LastName],
            [TypeEmployeeId]	
        )
        VALUES (
			@EmployeeCardNumber,
            @FirstName,
            @MiddleInitial,
            @LastName,
            @EmployeeTypeId
        );

		SET @EmployeeId = SCOPE_IDENTITY();
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- Update employee

CREATE OR ALTER PROC [dbo].[spUpdateEmployee]
	@EmployeeId AS INT,
	@EmployeeCardNumber AS NVARCHAR(4),
	@FirstName AS NVARCHAR(30),
	@MiddleInitial AS NVARCHAR(1),
	@LastName AS NVARCHAR(30),
	@EmployeeTypeId AS INT,
	@RecordVersion ROWVERSION
AS
BEGIN
	
	BEGIN TRY
	DECLARE @CurrentRecordVersion ROWVERSION = (SELECT Employee.RecordVersion FROM Employee WHERE EmployeeId = @EmployeeId);
        IF @RecordVersion <> @CurrentRecordVersion
            THROW 51002, 'The record has been updated since you last retrieved it.', 1;


	BEGIN TRANSACTION
		
		 UPDATE Employee
			SET
				EmployeeCardNumber = @EmployeeCardNumber,
				FirstName = @FirstName,
				MiddleInitial = @MiddleInitial,
				LastName = @LastName,
				TypeEmployeeId = @EmployeeTypeId
			WHERE 
				EmployeeId = @EmployeeId;
      

		SET @EmployeeId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO


-- Restore employee

CREATE OR ALTER PROC [dbo].[spRetoreEmployee]
	@EmployeeId AS INT,
	@EmployeeTypeId AS INT,
	@RecordVersion ROWVERSION
AS
BEGIN
	
	BEGIN TRY
	DECLARE @CurrentRecordVersion ROWVERSION = (SELECT Employee.RecordVersion FROM Employee WHERE EmployeeId = @EmployeeId);
        IF @RecordVersion <> @CurrentRecordVersion
            THROW 51002, 'The record has been updated since you last retrieved it.', 1;


	BEGIN TRANSACTION
		
		 UPDATE Employee
			SET
				TypeEmployeeId = @EmployeeTypeId
			WHERE 
				EmployeeId = @EmployeeId;
      

		SET @EmployeeId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- Scan procedures
-------------------------------

-- Get all scan types

CREATE OR ALTER PROC [dbo].[spGetTypeScan]
AS
BEGIN
	BEGIN TRY
		SELECT * FROM ScanType
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get all Scans by employee id

CREATE OR ALTER PROC [dbo].[spSearchScanByEmployeeId]
@EmployeeId AS INT
AS
BEGIN
	BEGIN TRY
		SELECT * FROM RawScan WHERE EmployeeId = @EmployeeId
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get Scan by id

CREATE OR ALTER PROC [dbo].[spGetScanById]
@ScanId AS INT
AS
BEGIN
	BEGIN TRY
		SELECT * FROM RawScan WHERE RawScanID = @ScanId
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get last Scans by employee id

CREATE OR ALTER PROC [dbo].[spSearchLastScanByEmployeeId]
@EmployeeId AS INT
AS
BEGIN
	BEGIN TRY
		SELECT TOP 1 * 
		FROM RawScan 
		WHERE EmployeeId = @EmployeeId
		Order By RawScanID Desc
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Get Scans by  Date

CREATE OR ALTER PROC [dbo].[spSearchScanByDate]
    @StartDate AS DATETIME2 = NULL,
    @EndDate AS DATETIME2 = NULL
AS
BEGIN
    BEGIN TRY
        SELECT *
        FROM RawScan
        WHERE (@StartDate IS NULL OR ScanDate >= @StartDate)
          AND (@EndDate IS NULL OR ScanDate <= @EndDate)
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- Add new scan

CREATE OR ALTER PROC [dbo].[spAddScan]
	@ScanId AS INT OUTPUT,
	@EmployeeId AS INT,
	@ScanDate AS DATETIME2,
	@ScanTypeId AS INT
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		
		 INSERT INTO [dbo].[RawScan] (
			[EmployeeId],
            [ScanDate],
            [ScanType]	
        )
        VALUES (
			@EmployeeId,
            @ScanDate,
            @ScanTypeId
        );

		SET @ScanId = SCOPE_IDENTITY();
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- Update scan

CREATE OR ALTER PROC [dbo].[spUpdateScan]
	@ScanId AS INT,
	@EmployeeId AS INT,
	@ScanDate AS DATETIME2,
	@ScanTypeId AS INT,
	@RecordVersion ROWVERSION
AS
BEGIN
	BEGIN TRY
		DECLARE @CurrentRecordVersion ROWVERSION = (SELECT RawScan.RecordVersion FROM RawScan WHERE RawScanID = @ScanId);
			IF @RecordVersion <> @CurrentRecordVersion
				THROW 51002, 'The record has been updated since you last retrieved it.', 1;
	
	BEGIN TRANSACTION
		UPDATE [dbo].[RawScan]
		SET	
			ScanDate = @ScanDate,
			EmployeeId = @EmployeeId,
			ScanType = @ScanTypeId
		WHERE 
			RawScanID = @ScanId


		SET @ScanId = SCOPE_IDENTITY();
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
	END CATCH
END

GO

-- get next, current and previous rows

CREATE OR ALTER PROC [dbo].[spGetLCNScanRows]
@ScanId AS INT,
@EmployeeId AS INT
AS
BEGIN
	BEGIN TRY
		WITH CTE AS (
			SELECT rownum = ROW_NUMBER() OVER (ORDER BY RawScanID),
			   RawScanID
			FROM RawScan
			WHERE EmployeeId = @EmployeeId
		)
		SELECT
			prev.RawScanID AS PreviousValue,
			CTE.RawScanID,
			nex.RawScanID AS NextValue
		FROM CTE
			LEFT JOIN CTE prev ON prev.rownum = CTE.rownum - 1
			LEFT JOIN CTE nex ON nex.rownum = CTE.rownum + 1
		WHERE CTE.RawScanID = @ScanId;



	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

-- Shift 

CREATE OR ALTER PROC [dbo].spGetShift
	@EmployeeId AS INT,
    @StartDate AS DATETIME2,
    @EndDate AS DATETIME2
AS
BEGIN
    BEGIN TRY
        SELECT *
        FROM RawScan
        WHERE (@StartDate IS NULL OR ScanDate >= @StartDate)
          AND (@EndDate IS NULL OR ScanDate <= @EndDate)
		  AND EmployeeId = @EmployeeId
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

