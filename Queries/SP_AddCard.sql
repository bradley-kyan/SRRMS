Create Procedure AddCard @Card_ID varchar(255), @HR_Code char(5), @F_Name varchar(100), @L_Name varchar(100)
AS
Begin
begin tran
	begin try
		Update SRRMS_DB.dbo.Student_RFID SET Card_ID = @Card_ID where S_UID = (select S_UID from SRRMS_DB.dbo.Student_Info where HR_Code = @HR_Code AND F_Name = @F_Name AND L_Name = @L_Name)
		commit
	end try
	begin catch
		rollback
	end catch
End