USE [BookStore]

CREATE TABLE WishList(
	UserId int,
	BookId int,
	WishListId int NOT NULL IDENTITY(1,1) PRIMARY KEY
);

SELECT * FROM [WishList]

Create PROC spAddWishList
@UserId INT ,
@BookId INT ,
@result int output
AS
BEGIN
BEGIN TRY
INSERT INTO WishList(
UserId,
BookId)
VALUES(
@UserId  ,
@BookId 
);
set @result=1;
END TRY
BEGIN CATCH
   set @result=0;
END CATCH
END

CREATE PROC spDeleteWishList
@WishListId INT ,
@result int output
AS
BEGIN
BEGIN TRY
Delete FROM WishList Where WishListId = @WishListId;
set @result=1;
END TRY
BEGIN CATCH
set @result=0;
END CATCH
END

Create PROC spGetWishList
(@userId INT)
AS
BEGIN
select 
Books.BookId,BookName,AuthorName,Price,OriginalPrice,Image,WishListId
FROM Books
inner join Wishlist
on WishList.BookId=Books.BookId where WishList.UserId=@userId
END