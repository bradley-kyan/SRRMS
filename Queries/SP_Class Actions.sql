Create proc ClassAdd @C_Name varchar(80), @C_DeviceID char(10), @C_Day date, @C_Period tinyint, @C_Override bit, @C_Description varchar(255), @C_Override_Start date, @C_Override_End date
As
begin
	Declare @CheckExist char(10) = (select C_DeviceID from SRRMS_DB.dbo.Devices where C_DeviceID = @C_DeviceID)
	IF @CheckExist != null
	begin
		Declare @Duplicate varchar (80) = (select C_Name from SRRMS_DB.dbo.Class_Info where C_Name = @C_Name)	
		if @Duplicate != null AND @C_Override = 1	
		begin
			begin tran
			Insert into SRRMS_DB.dbo.Class_Info (C_Name, C_DeviceID, C_Day, C_Period, C_Description, C_Override, C_Override_Start, C_Override_End) VALUES (@C_Name, @C_DeviceID, @C_Day, @C_Period, @C_Description, @C_Override, @C_Override_Start, @C_Override_End)
			commit
			return
		end
		else if @Duplicate != null AND @C_Override = 0
		begin
			return
		end
		else
		begin
			begin tran
			Insert into SRRMS_DB.dbo.Class_Info (C_Name, C_DeviceID, C_Day, C_Period, C_Description, C_Override) VALUES (@C_Name, @C_DeviceID, @C_Day, @C_Period, @C_Description, @C_Override)
			commit
			return
		end
	end
	Else
	Begin
		print 'DeviceID does not exist'
		return
	End
end
GO

Create proc ClassPurge @Class_Name varchar(80)
As
Begin
	if @Class_Name is null
	begin
		begin tran
		delete from SRRMS_DB.dbo.Class_Info
		commit
		return
	end
	else
	begin
		Declare @CheckExist varchar(80) = (select C_Name from SRRMS_DB.dbo.Class_Info where C_Name = @Class_Name)
		if @CheckExist != null
		begin
			begin tran
			delete from SRRMS_DB.dbo.Class_Info where C_Name = @Class_Name
			commit
		end
	end
end

GO

Create proc ClassGet @Overide bit
AS
Begin
	if @Overide is null
		begin
		Select * from SRRMS_DB.dbo.Class_Info order by C_Override ASC, C_Name DESC
		end
	else
		begin
			Select * from SRRMS_DB.dbo.Class_Info where C_Override = @Overide order by C_Name DESC
		end
	return
End

-- drop proc ClassAdd
-- drop proc ClassPurgeAll
