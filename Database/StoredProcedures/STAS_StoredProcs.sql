/*
NAME: Andrii Lebid
DATE: 05/27/2024
PURPOSE: Advatek System
Description: Simple Time And Attendance System / Stored Procedures
Version: 1.3
*/


USE [STAS-A];
GO

-- User Login

CREATE OR ALTER PROC [dbo].[spLogin]
@UserName AS NVARCHAR(30),
@Password AS NVARCHAR(64)

AS
BEGIN
	BEGIN TRY
		SELECT * FROM [User] 
		WHERE [User].UserName = @UserName AND [User].[Password] = @Password
	END TRY
	BEGIN CATCH
	;THROW
	END CATCH
END

GO

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


-- Employee procedures


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

-- Scan procedures

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


