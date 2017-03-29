ALTER proc [dbo].[Jobs_DeleteById]
	@Id int

as


Begin

	Delete dbo.Jobs
	Where Id = @Id

End