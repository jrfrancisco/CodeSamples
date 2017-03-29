USE [C28]
GO
/****** Object:  StoredProcedure [dbo].[Reports_UserRegistrationReport_Update]    Script Date: 3/28/2017 9:25:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Reports_UserRegistrationReport_Update]
		   @Date date
		 , @WebsiteId int

AS

BEGIN
	DECLARE 
		@StartDate as datetime2(7) 
		, @EndDate as datetime2(7)
		, @TotalRegistered as int 
		, @TotalReferrals as int  

	SET @StartDate = (CAST(@Date as datetime2(7)))

	SET @EndDate = DATEADD(DAY, DATEDIFF(DAY, -1, @Date), 0)

	SET @TotalRegistered = (
		SELECT 
			COUNT(p.[UserId])
		FROM  
			[dbo].[UserProfiles] p
			INNER JOIN
			[dbo].[UserWebsite] w
				ON p.UserId = w.UserId
		WHERE 
			p.[DateAdded] >= @StartDate AND p.[DateAdded] <= @EndDate and w.WebsiteId = @WebsiteId)

	SET @TotalReferrals = (
		SELECT 
			COUNT([TokenHash]) 
		FROM  
			[dbo].[UserProfiles] p
			INNER JOIN
			[dbo].[UserWebsite] w 
			ON p.UserId = w.UserId
	
		WHERE 
			p.[DateAdded] >= @StartDate AND p.[DateAdded] <= @EndDate and w.WebsiteId = @WebsiteId and [TokenHash] is not null)

   	IF EXISTS 
		(SELECT 1 FROM [dbo].[Report.UserRegistration] 

		WHERE 
			[Date] = @Date AND [WebsiteId] = @WebsiteId)


	BEGIN
		UPDATE 
			[dbo].[Report.UserRegistration]
		SET		   
			[TotalRegistered] = @TotalRegistered
			,[TotalReferrals] = @TotalReferrals
		WHERE
			@Date = [Date]
			AND
			@WebsiteId = [WebsiteId]
	END

	ELSE 

	BEGIN
		INSERT INTO [dbo].[Report.UserRegistration]
			   ([Date]
			   ,[WebsiteId]
			   ,[TotalRegistered]
			   ,[TotalReferrals])
		VALUES
			   (@Date
			   ,@WebsiteId
			   ,@TotalRegistered
			   ,@TotalReferrals)
	END
END


SELECT
	 [Date]
	,[WebsiteId]
	,[TotalRegistered]
	,[TotalReferrals]

FROM [dbo].[Report.UserRegistration]

ORDER BY [WebsiteId], [Date]
