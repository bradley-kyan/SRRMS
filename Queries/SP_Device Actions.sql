Create procedure DeviceAorE @DeviceID char(10), @ClassDesc varchar(50)
As
Begin
	If (select C_DeviceID from SRRMS_DB.dbo.Devices where C_DeviceID = @DeviceID) = @DeviceID
	Begin
		if (select Class_Description from SRRMS_DB.dbo.Devices where Class_Description = @ClassDesc) != @ClassDesc
		Begin
			begin tran
			Update SRRMS_DB.dbo.Devices SET Class_Description = @ClassDesc Where C_DeviceID = @DeviceID
			commit
			Print 'Updated Class Description'
			Return
		End
		Print 'Device Already Exists'
		rollback
		Return
	End
	Else
	Begin
		begin tran
		Insert into SRRMS_DB.dbo.Devices (C_DeviceID, Class_Description) Values (@DeviceID, @ClassDesc)
		commit
		Print 'Successfully Added Device: ' + @DeviceID + ' ||  Description: ' + @ClassDesc
		Return
	End
End

go

Create proc DeviceRem @DeviceID char(10)
As
begin
	begin tran
	Delete from SRRMS_DB.dbo.Devices Where C_DeviceID = @DeviceID
	commit
end
go

Create Proc DeviceSearch @ClassDesc varchar(50)
AS
Begin
	Select * from SRRMS_DB.dbo.Devices where Class_Description like '%'+@ClassDesc+'%'
End

-- Delete Proc DeviceRem
-- Delete Proc DeviceSearch
-- Delete Proc DeviceAorE