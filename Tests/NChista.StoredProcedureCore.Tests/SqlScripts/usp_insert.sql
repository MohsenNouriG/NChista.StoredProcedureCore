CREATE PROCEDURE usp_insert
	@Id int
	, @FirstName nvarchar(50)
	, @LastName nvarchar(50)
	, @Birthdate date
	, @EmailAddress nvarchar(50)
AS
BEGIN
	
	INSERT INTO TestStudent
           ([Id]
           ,[FirstName]
           ,[LastName]
           ,[Birthdate]
           ,[EmailAddress])
     VALUES
           (@Id
           ,@FirstName
           ,@LastName
           ,@Birthdate
           ,@EmailAddress)

END
