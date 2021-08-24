Create procedure SP_DeviceAorE @DeviceID char(10), @ClassDesc varchar(50)
As
Begin
	If (select C_DeviceID from SRRMS_DB.dbo.Devices where C_DeviceID = @DeviceID) = @DeviceID
	Begin
		if (select Class_Description from SRRMS_DB.dbo.Devices where Class_Description = @ClassDesc) != @ClassDesc
		Begin
			Update SRRMS_DB.dbo.Devices SET Class_Description = @ClassDesc Where C_DeviceID = @DeviceID
			Print 'Updated Class Description'
			Return
		End
		Print 'Device Already Exists'
		Return
	End

	Else
	Begin
		Insert into SRRMS_DB.dbo.Devices (C_DeviceID, Class_Description) Values (@DeviceID, @ClassDesc)
		Print 'Successfully Added Device: ' + @DeviceID + ' ||  Description: ' + @ClassDesc
		Return
	End
End

go

Create proc SP_DeviceRem @DeviceID char(10)
As
begin
	Delete from SRRMS_DB.dbo.Devices Where C_DeviceID = @DeviceID
end
go

Create Proc SP_DeviceSearch @ClassDesc varchar(50)
AS
Begin
	Select * from SRRMS_DB.dbo.Devices where Class_Description like '%'+@ClassDesc+'%'
End

-- Delete Proc SP_DeviceRem
-- Delete Proc SP_DeviceSearch
-- Delete Proc SP_DeviceAorE