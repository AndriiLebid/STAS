/*
NAME: Andrii Lebid
DATE: 05/27/2024
PURPOSE: Advatek System
Description: Simple Time And Attendance System / Stored Procedures
Version: 1.0
*/


USE [STAS-A];
GO

CREATE OR ALTER PROC [dbo].[OrderRecordVersion]
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

