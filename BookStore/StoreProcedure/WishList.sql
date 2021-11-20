USE [BookStore]

CREATE TABLE WishList(
	UserId int,
	BookId int,
	WishListId int NOT NULL IDENTITY(1,1) PRIMARY KEY
);

SELECT * FROM [WishList]

Create PROC storeprocedureAddWishList
@UserId INT ,
@BookId INT 

AS
BEGIN
INSERT INTO WishList(
UserId,
BookId)
VALUES(
@UserId  ,
@BookId 
);
END

CREATE PROC storeprocedureDeleteWishList
@WishListId INT 

AS
BEGIN
Delete FROM WishList Where WishListId = @WishListId;
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