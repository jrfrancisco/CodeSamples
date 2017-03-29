ALTER Proc [dbo].[Jobs_Insert]
	@Id int Output
	,@UserId nvarchar(128) = null
	,@Status int = null
	,@JobType int = null
	,@Price decimal(11,2) = null
	,@Phone varchar(30) = null
	,@JobRequest int = null
	,@SpecialInstructions nvarchar(MAX) = null
	,@WebsiteId int = null
	,@ExternalCustomerId int = null

as


Begin

INSERT INTO [dbo].[Jobs]
	([UserId]
	,[Status]
        ,[JobType]
        ,[Price]
        ,[Phone]
	,[JobRequest]
	,[SpecialInstructions]
	,[WebsiteId]
	,[ExternalCustomerId])

Values
	(@UserId
	,@Status
	,@JobType
	,@Price
	,@Phone
	,@JobRequest
	,@SpecialInstructions
	,@WebsiteId
	,@ExternalCustomerId)

	Set @Id = SCOPE_IDENTITY()

End