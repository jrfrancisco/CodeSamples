ALTER proc [dbo].[Jobs_Update]
	@Id int 
	,@UserId nvarchar(128)
	,@Status int 
	,@JobType int 
	,@Price decimal(11,2)
	,@Phone varchar(30)
	,@JobRequest int 
	,@SpecialInstructions nvarchar(MAX) 
	,@WebsiteId int 
	,@ExternalJobId int 
	,@ExternalCustomerId int 
	,@PaymentNonce nvarchar(128)
	,@BillingId int

as


Begin
	
	Update dbo.Jobs
	Set 
		[Modified] = getutcdate()
		,[UserId] = @UserId
		,[Status] = @Status
		,[JobType] = @JobType
		,[Price] = @Price
		,[Phone] = @Phone
		,[JobRequest] = @JobRequest
		,[SpecialInstructions] = @SpecialInstructions
		,[WebsiteId] = @WebsiteId
		,[ExternalJobId] = @ExternalJobId
		,[ExternalCustomerId] = @ExternalCustomerId
		,[PaymentNonce] = @PaymentNonce
		,[BillingId] = @BillingId		
	Where Id = @Id
		
End