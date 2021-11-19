USE [BookStore]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE CustomerFeedback(
	BookId int,
	UserId int ,
	Rating float,
	FeedBack varchar (1000) 
	);

SELECT * FROM [CustomerFeedback]

CREATE PROCEDURE storeprocedureAddFeedback
	
	@BookId int,
	@UserId int ,
	@Rating float,
	@FeedBack varchar (1000) 
	
AS
	BEGIN
		INSERT into CustomerFeedback(
		
		BookId,
		UserId,
		rating,
		Feedback
		)

		values
		(
			@BookId ,
			@UserId  ,
			@Rating,
			@FeedBack 

		)
	END
RETURN 0

CREATE PROCEDURE StoreProcedurGetCustomerFeedback
   @bookid int
 AS
 BEGIN
    SELECT UserS._id,FullName,Feedback,Rating
	from Users 
	inner join CustomerFeedback 
	on CustomerFeedback.UserId=UserS._id where CustomerFeedback.BookId=@bookid
 end