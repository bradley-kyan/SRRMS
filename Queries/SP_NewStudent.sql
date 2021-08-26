Create Procedure NewStudent @F_Name	varchar(100), @L_Name varchar(100), @HR_Code char(5), @Y_Level tinyint, @Subject_List varchar(max), @Card_ID varchar(max)
AS
Begin
	if @F_Name is null OR @L_Name is null OR @HR_Code is null OR @Y_Level is null
	begin
		return
	end
	if @Card_ID is null
	begin
		begin tran
		declare @scope int;	
		Insert into SRRMS_DB.dbo.Student_RFID (Card_ID) Values (null)
		SELECT @scope = SCOPE_IDENTITY()
		Declare @uid uniqueidentifier = (select S_UID from SRRMS_DB.dbo.Student_RFID where SR_ID = @scope)
		Insert into SRRMS_DB.dbo.Student_Info (F_Name, L_Name, HR_Code, Y_Level, Subject_List, S_UID) Values (@F_Name, @L_Name, @HR_Code, @Y_Level, @Subject_List, @uid)
		commit
	end
	else
	begin
		begin tran
		begin try
			Insert into SRRMS_DB.dbo.Student_RFID (Card_ID) Values (@Card_ID)
		end try
		begin catch
		end catch
		Insert into SRRMS_DB.dbo.Student_Info (F_Name, L_Name, HR_Code, Y_Level, Subject_List, S_UID) Values (@F_Name, @L_Name, @HR_Code, @Y_Level, @Subject_List, (select S_UID from SRRMS_DB.dbo.Student_RFID where Card_ID = @Card_ID))
		commit
	end
End

-- drop Proc NewStudent