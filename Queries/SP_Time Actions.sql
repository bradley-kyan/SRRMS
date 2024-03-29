Create Proc StringToDate (@date varchar(50), @ReturnDate date out)
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

Create Proc DateToString (@date varchar(50), @ReturnDate date out)
AS
begin
	select @ReturnDate = datename(dw, convert(date, @date))
	return
end

go

Create Proc TimesAdd @T_Day varchar(50), @Period_Time1 time, @Period_End1 time, @Period_Time2 time, @Period_End2 time, @Period_Time3 time, @Period_End3 time, 
	@Period_Time4 time, @Period_End4 time, @Period_Time5 time, @Period_End5 time
AS
Begin
	if @T_Day is null
		begin
			return
		end
	declare @date date
	begin try
		exec StringToDate @T_Day, @date OUTPUT
	end try
	begin catch
		return
	end catch
	Declare @verify date = (select T_Day from SRRMS_DB.dbo.Times where T_Day = @date)
	if @verify is null
	begin
		begin tran
		Insert into SRRMS_DB.dbo.Times (T_Day, Period_End1, Period_End2, Period_End3, Period_End4, Period_End5, Period_Time1, Period_Time2, Period_Time3, Period_Time4, Period_Time5) 
			Values (@date, @Period_End1, @Period_End2, @Period_End3, @Period_End4, @Period_End5, @Period_Time1, @Period_Time2, @Period_Time3, @Period_Time4, @Period_Time5)
		commit
	end
	else
	begin
		begin tran
		Update SRRMS_DB.dbo.Times set Period_End1 = @Period_End1, Period_End2 = @Period_End2, Period_End3 = @Period_End3, Period_End4 = @Period_End4, Period_End5 = @Period_End5,
			Period_Time1 = @Period_Time1, Period_Time2 = @Period_Time2, Period_Time3 = @Period_Time3, Period_Time4 = @Period_Time4, Period_Time5 = @Period_Time5
		where T_Day = @date
		commit
	end
End

GO

Create Proc TimesOveride @T_Day varchar(50), @T_OverrideBegin char(9), @T_OverrideEnd char(9), @Period_Time1 time, @Period_End1 time, @Period_Time2 time, @Period_End2 time,
	@Period_Time3 time, @Period_End3 time, @Period_Time4 time, @Period_End4 time, @Period_Time5 time, @Period_End5 time
AS
Begin
	if @T_Day is null OR @T_OverrideBegin is null OR @T_OverrideEnd is null
		begin
			return
		end
	else
	begin
		declare @Tdate date
		begin try
			exec StringToDate @T_Day, @Tdate OUTPUT
			end try
		begin catch
			print 'An error has occured'
			return
		end catch
		begin tran
		Insert into SRRMS_DB.dbo.Time_Override(T_Day, Period_End1, Period_End2, Period_End3, Period_End4, Period_End5, Period_Time1, Period_Time2, Period_Time3, Period_Time4, Period_Time5, T_OverrideBegin, T_OverrideEnd) 
			Values (@Tdate, @Period_End1, @Period_End2, @Period_End3, @Period_End4, @Period_End5, @Period_Time1, @Period_Time2, @Period_Time3, @Period_Time4, @Period_Time5, @T_OverrideBegin, @T_OverrideEnd)
		commit
	end
End