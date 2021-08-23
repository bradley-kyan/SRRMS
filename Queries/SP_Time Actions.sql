Create Proc SP_TimesAdd @T_Day char(9) not null, @Period_Time1 time, @Period_End1 time, @Period_Time2 time, @Period_End2 time, @Period_Time3 time, @Period_End3 time, 
	@Period_Time4 time, @Period_End4 time, @Period_Time5 time, @Period_End5 time
AS
Begin
	declare @date date
	begin try
	exec SP_StringToDate @T_Day, @date OUTPUT
	end try
	begin catch
		Print 'Invalid T_Day'
		return
	end catch
	Declare @verify date = (select T_Day from Times where T_Day = @date)
	if @verify is null
		Insert into SRRMS_DB.dbo.Times (T_Day, Period_End1, Period_End2, Period_End3, Period_End4, Period_End5, Period_Time1, Period_Time2, Period_Time3, Period_Time4, Period_Time5) 
			Values (@date, @Period_End1, @Period_End2, @Period_End3, @Period_End4, @Period_End5, @Period_Time1, @Period_Time2, @Period_Time3, @Period_Time4, @Period_Time5)
	else
		Update Times set Period_End1 = @Period_End1, Period_End2 = @Period_End3, Period_End3 = @Period_End3, Period_End4 = @Period_End4, Period_End5 = @Period_End5,
			Period_Time1 = @Period_Time1, Period_Time2 = @Period_Time2, Period_Time3 = @Period_Time3, Period_Time4 = @Period_Time4, Period_Time5 = @Period_Time5
		where T_Day = @T_Day
	return
End

GO

Create Proc SP_TimesOveride @T_Day char(9) not null, @T_OverrideBegin char(9) not null, @T_OverrideEnd char(9) not null, @Period_Time1 time, @Period_End1 time, @Period_Time2 time, @Period_End2 time,
	@Period_Time3 time, @Period_End3 time, @Period_Time4 time, @Period_End4 time, @Period_Time5 time, @Period_End5 time
AS
Begin
	declare @Tdate date
	declare @OvrBegin date
	declare @OvrEnd date
	begin try
		exec SP_StringToDate @T_Day, @Tdate OUTPUT
		exec SP_StringToDate @T_OverrideBegin, @OvrBegin OUTPUT
		exec SP_StringToDate @T_OverrideEnd, @OvrEnd OUTPUT
		end try
	begin catch
		print 'An error has occured'
		return
	end catch

End

GO


/*
declare @ReturnDate date
exec SP_StringToDate 'tue', @ReturnDate OUTPUT
Select @ReturnDate
*/

--drop proc SP_StringToDate

Create Proc SP_StringToDate (@date char(9), @ReturnDate date out)
AS
begin
	declare @tryconvert char(9)
	begin try
		Set @tryconvert = datename(dw, convert(date, @date))
	end try
	begin catch
		Set @tryconvert = @date
	end catch
	
	select @ReturnDate =
	case @tryconvert
		when 'monday' then '1-5-1970 00:00:00'
		when 'mon' then '1-5-1970 00:00:00'
		when 'm' then '1-5-1970 00:00:00'

		when 'tuesday' then '1-6-1970 00:00:00'
		when 'tue' then '1-6-1970 00:00:00'
		when 't' then '1-6-1970 00:00:00'
		when 'tu' then '1-6-1970 00:00:00'

		when 'wednesday' then '1-7-1970 00:00:00'
		when 'wed' then '1-7-1970 00:00:00'
		when 'w' then '1-7-1970 00:00:00'

		when 'thursday' then '1-1-1970 00:00:00'
		when 'thurs' then '1-1-1970 00:00:00'
		when 'th' then '1-1-1970 00:00:00'

		when 'friday' then '1-2-1970 00:00:00'
		when 'fri' then '1-2-1970 00:00:00'
		when 'f' then '1-2-1970 00:00:00'

		when 'saturday' then '1-3-1970 00:00:00'
		when 'sat' then '1-3-1970 00:00:00'
		when 'sa' then '1-3-1970 00:00:00'

		when 'sunday' then '1-4-1970 00:00:00'
		when 'sun' then '1-4-1970 00:00:00'
		when 'su' then '1-4-1970 00:00:00'

		end
	return
end

go

Create Proc SP_DateToString (@date char(9), @ReturnDate date out)
AS
begin
	select @ReturnDate = datename(dw, convert(date, @date))
	return
end
