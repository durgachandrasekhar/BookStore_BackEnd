USE [BookStore]
GO
/****** Object:  StoredProcedure [dbo].[storeprocedureAddBook]    Script Date: 18-11-2021 10:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[storeprocedureAddBook]

	@BookName VARCHAR(250),
	@AuthorName VARCHAR(250),
	@Price int,
	@BookDescription VARCHAR(500),
	@Image VARCHAR(100),
	@Rating int,
	@BookCount int,
	@OriginalPrice int
AS
BEGIN
INSERT INTO BOOKS(
	BookName,
	AuthorName,
	Price,
	BookDescription,
	Image,
	Rating,
	BookCount,
	OriginalPrice
)
	Values(
		@BookName,
		@AuthorName,
		@Price,
		@BookDescription,
		@Image,
		@Rating,
		@BookCount,
		@OriginalPrice
	)

END

CREATE PROC spGetAllBooks
AS
BEGIN
BEGIN TRY
Select * from Books
END TRY
BEGIN CATCH
END CATCH
END