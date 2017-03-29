GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER proc [dbo].[Jobs_UpdatePrice]

 @Id int
 ,@Price decimal(11,2)

as

BEGIN


UPDATE [dbo].[Jobs]
   SET 
      [Price] = @Price
   
 WHERE Id = @Id


 End