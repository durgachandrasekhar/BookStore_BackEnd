USE [BookStore]

CREATE TABLE UserS
(
	_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	fullName VARCHAR(255) NOT NULL,
	email VARCHAR(255) NOT NULL,
	password VARCHAR(255) NOT NULL,
	phone BigInt
);

SELECT * FROM [UserS]

CREATE PROCEDURE StoreProcedureUserRegisteration
	@fullName VARCHAR(255),
	@email VARCHAR(255),
	@password VARCHAR(255),
	@phone BigInt,
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM UserS WHERE email = @email)
		SET @user = NULL;
	ELSE
		INSERT INTO UserS(fullName, email, password, phone)
		VALUES (@fullName, @email, @password,@phone)
		SET @user = SCOPE_IDENTITY();
END

CREATE PROCEDURE StoreProcedurLogin
	@email VARCHAR(255),
	@password VARCHAR(255),
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM UserS WHERE email = @email)
	BEGIN
		IF EXISTS(SELECT * FROM UserS WHERE email = @email AND password = @password)
		BEGIN
		SET @user = 2;
		END
		ELSE
		BEGIN
		SET @user = 1;
		END
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END

CREATE PROCEDURE spForgotPassword
	@email VARCHAR(255),
	@user INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM UserS WHERE email = @email)
	BEGIN
		SELECT @user = _id FROM UserS WHERE email = @email;
	END
	ELSE
	BEGIN
		SET @user = NULL;
	END
END

CREATE PROC ResetPasssword
	@id INT,
	@password VARCHAR(25),	
	@result INT OUTPUT
AS
BEGIN
BEGIN TRY
	UPDATE UserS
	SET 		
		UserS.password = @password
	WHERE
		_id = @id;
		set @result=1;
END TRY
BEGIN CATCH
set @result=0;
END CATCH
END

