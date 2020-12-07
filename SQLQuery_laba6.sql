CREATE DATABASE Laba6
GO

USE Laba6

CREATE TABLE Instruments
(
	Id int IDENTITY NOT NULL,
	NameFile varchar(50) NOT NULL,
	Mark varchar(50) NOT NULL,
	Name varchar(50) NOT NULL,
	ImageData varbinary(max) NOT NULL
)
GO

CREATE PROCEDURE AddInstrument
	@NameFile varchar(50),
	@Mark varchar(50),
	@Name varchar(50),
	@ImageData varbinary(max)
AS
	INSERT INTO Instruments (NameFile, Mark, Name, ImageData)
	VALUES (@NameFile, @Mark, @Name, @ImageData)
	SELECT SCOPE_IDENTITY()
GO

