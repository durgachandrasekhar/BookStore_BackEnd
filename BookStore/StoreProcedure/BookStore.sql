USE [BookStore]

CREATE TABLE BOOKS(
	Bookid int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	BookName VARCHAR(250),
	AuthorName VARCHAR(250),
	Price int,
	BookDescription VARCHAR(500),
	Image VARCHAR(100),
	Rating int,
	BookCount int,
	OriginalPrice int
);

SELECT * FROM [BOOKS]
	
CREATE PROC storeprocedureAddBook

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

CREATE PROCEDURE storeprocedureUpdateBook
      @BookId int,
	  @originalPrice int,
	  @BookName varchar(250),
	  @AuthorName varchar(250),
	  @Price int,
	  @BookDescription varchar(500),
	  @Image varchar(100),
	  @Rating int,
	  @BookCount int,
	  @result int output
AS
BEGIN
	if Exists(select * from Books where Bookid=@BookId)
	 begin
		Update  Books 
		set
		  BookName =@BookName,
		  AuthorName=@AuthorName,
		  Price = @Price,
		  BookDescription=@BookDescription,
		  Image =@Image,
		  Rating =@Rating,
		  BookCount=@BookCount,
		  OriginalPrice=@originalPrice

  where Bookid=@BookId;
  set @result=1
  end
  else
  begin
   set @result=0;
   end
END

CREATE PROCEDURE StoreprocedureGetBook
  @BookId int
AS
BEGIN

     IF(EXISTS(SELECT * FROM Books WHERE Bookid=@BookId))
	 begin
	   SELECT * FROM Books WHERE Bookid=@BookId;
   	 end
	 else
	   THROW  52000, 'Book Not Available', 1;
End

CREATE PROC storeprocedureRemoveBook
@BookId INT ,
@result int output
AS
BEGIN

    if exists(select * from Books where Bookid=@BookId)
	begin
       Delete FROM Books Where Bookid=@BookId;
	   set @result=1;
	end
	else
	 begin
	   set @result=0;
     end
END