Create proc SP_ClassAdd
As
begin
	
	return
end

GO

Create proc SP_ClassPurgeAll
As
Begin
	delete from Class_Info
end