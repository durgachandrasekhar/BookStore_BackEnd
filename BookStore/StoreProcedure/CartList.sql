USE [BookStore]
GO
/****** Object:  StoredProcedure [dbo].[AddToCart]    Script Date: 9/29/2021 1:06:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE CartList(
	CartId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	BookId int,
	UserId int,
	NoOfBook int
);

SELECT * FROM [CartList]

CREATE procedure storeprocedureAddToCart
 @BookId int,
 @UserId int,
 @NoOfBook int,
 @result int output

 as
 begin
  BEGIN TRY
      if Exists(select * from CartList where UserId=@UserId and BookId=@BookId)
	     begin
		   set @result=0;
		 end
	  else
	     begin
		   insert into CartList values (@BookId,@UserId,@NoOfBook)
		   update Books
				   set Books.BookCount=Books.BookCount-1
				   where BookId=@BookId;
		   set @result=1;
		 end
  END TRY
  begin catch
      set @result=0;
  end catch
 end

CREATE procedure spUpdateCart
 @CartId int,
 @type bit,
 @result int output

 as
 begin
  BEGIN TRY
  BEGIN Transaction
  declare @count  int , @bookid int;

      if Exists(select * from CartList where CartId=@CartId)
	     begin
		   select @count=NoOfBook,@bookid=BookId from CartList where CartId=@CartId; 
		   if(@type=1)		
 begin
			   if exists(select * from Books where Books.BookCount !=0 and BookId=@bookid)
			    begin
					update CartList
				   set NoOfBook=@count+1
				   where CartId=@CartId;
				   update Books
				   set Books.BookCount=Books.BookCount-1
				   where BookId=@bookid;
				   set @result=1;
				end
				else
				 begin
				  set @result=0;
				 end
			 end
			 else
			   begin
			     update CartList
				   set NoOfBook=@count-1
				   where CartId=@CartId;
				  update Books
				   set Books.BookCount=Books.BookCount+1
				   where BookId=@bookid;
				   set @result=1;
			   end
			end
	  else
	     begin
		   set @result=0;
		 end
   if(@result=1)
	      begin
		  Commit Tran
		end

  END TRY
  begin catch
  set @result=0;
  Rollback Tran
  end catch
 end

CREATE procedure spGetCartItem
 @userId int

as
begin
    select Books.BookId,BookName,AuthorName,Price,OriginalPrice,CartId,CartList.NoOfBook,Books.BookCount,Image,CartList.Userid 
	from Books 
	inner join CartList 
	on CartList.BookId=Books.BookId where CartList.Userid=@userId
end

CREATE procedure spRemoveFromCart
 @CartId int,
 @result int output

 as
 begin
  BEGIN TRY
  declare @count  int,@bookid int
      if Exists(select  * from CartList where CartId=@CartId)
	     begin
			select  @count=NoOfBook,@bookid=BookId from CartList where CartId=@CartId
		   DELETE FROM CartList WHERE CartId=@CartId;
		   update Books
				   set Books.BookCount=Books.BookCount+@count
				   where BookId=@bookId;
		   set @result=1;
		 end
	  else
	     begin
		   set @result=0;
		 end
  END TRY
  begin catch
      set @result=0;
  end catch
 end