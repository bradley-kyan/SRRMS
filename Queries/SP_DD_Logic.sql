Create Proc ReseedDD
As
Begin
	Begin transaction
		begin try
			Delete from SRRMS_DB.dbo.Data_Dump
			DBCC CHECKIDENT ('SRRMS_DB.dbo.Data_Dump', RESEED, 0)
			commit
			Print 'Reseeded Data_Dump table'
			return
		end try
		begin catch
			rollback
			return
		end catch
end

GO

Create Proc GetPeriod @TimeInput time, @PeriodOutput tinyint OUTPUT, @Late bit OUTPUT
AS
Begin
	Declare @LateValueInMin int = 7
	Declare @getdate datetime
	declare @now datetime = getdate()
	Exec StringToDate @now, @getdate OUTPUT 
	print @getdate
	Declare @override datetime = (select T_Day from SRRMS_DB.dbo.Time_Override where T_Day = @getdate)

	if @override is null
		begin
			if @TimeInput between (select Period_Time1 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End1 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time1 from SRRMS_DB.dbo.Times where T_Day = @getdate))
						begin
							select @PeriodOutput = 1
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 1
							select @Late = 0
							return
						end
				end
			else if @TimeInput between (select Period_Time2 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End2 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time2 from SRRMS_DB.dbo.Times where T_Day = @getdate))
						begin
							select @PeriodOutput = 2
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 2
							select @Late = 0
							return
						end
				end
			else if @TimeInput between (select Period_Time3 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End3 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time3 from SRRMS_DB.dbo.Times where T_Day = @getdate))
						begin
							select @PeriodOutput = 3
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 3
							select @Late = 0
							return
						end
				end
			else if @TimeInput between (select Period_Time4 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End4 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time4 from SRRMS_DB.dbo.Times where T_Day = @getdate))
						begin
							select @PeriodOutput = 4
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 4
							select @Late = 0
							return
						end
				end
			else if @TimeInput between (select Period_Time5 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End5 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time5 from SRRMS_DB.dbo.Times where T_Day = @getdate))
						begin
							select @PeriodOutput = 5
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 5
							select @Late = 0
							return
						end
				end

		end
	else
	begin
		if @now between (select T_OverrideBegin from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override and T_Day = @getdate) AND (select T_OverrideEnd from SRRMS_DB.dbo.Time_Override where T_OverrideEnd = @override and T_Day = @getdate)
			begin
				if @TimeInput between (select Period_Time1 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End1 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time1 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
						begin
							select @PeriodOutput = 1
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 1
							select @Late = 0
							return
						end
				else if @TimeInput between (select Period_Time2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
						begin
							select @PeriodOutput = 2
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 2
							select @Late = 0
							return
						end
				else if @TimeInput between (select Period_Time3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
						begin
							select @PeriodOutput = 3
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 3
							select @Late = 0
							return
						end
				else if @TimeInput between (select Period_Time4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
						begin
							select @PeriodOutput = 4
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 4
							select @Late = 0
							return
						end
				else if @TimeInput between (select Period_Time5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
						begin
							select @PeriodOutput = 5
							select @Late = 1
							return
						end
					else
						begin
							select @PeriodOutput = 5
							select @Late = 0
							return
						end
				else	
					return
			end
		else
			return
	end
End

GO

Create procedure DD_Logic
AS
BEGIN
	set nocount on
	
	declare @DD varchar = (Select DD_ID From SRRMS_DB.dbo.Data_Dump where DD_ID = '1')
			IF @DD is null
			begin
				Print 'Data_Dump table is empty'
				return
			end
			else
			begin

				select * into #Data_Dump_Temp from SRRMS_DB.dbo.Data_Dump
				Exec ReseedDD

				Declare @value int = 1
				Declare @max int
				Set @Max = (select DD_ID from #Data_Dump_Temp where DD_ID = (select max(DD_ID) from #Data_Dump_Temp) ) + 1
					While (1 = 1)
					Begin
						if @value = @max break;
						else
						begin
								begin tran
									begin try
										Declare @In_Time datetime = (select In_Time from #Data_Dump_Temp where DD_ID = @value)
										Declare @C_DeviceID varchar(255) = (select C_DeviceID from #Data_Dump_Temp where DD_ID = @value)
										Declare @Period tinyint
										Declare @Late bit
										Exec GetPeriod @In_Time, @Period OUTPUT, @Late OUTPUT
										Print 'Period: ' + convert(varchar(50), @Period)
										Print 'Late: ' + convert(varchar(50), @Late)
										Declare @S_UID uniqueidentifier = (select S_UID from SRRMS_DB.dbo.Student_RFID where Card_ID = (select Card_ID from #Data_Dump_Temp where DD_ID = @value))
										if @S_UID is not null
											begin
												Print 'S_UID: ' + convert(varchar(50), @S_UID)
												insert into SRRMS_DB.dbo.Attendence (S_UID, C_DeviceID, Real_TimeIn, A_Period, A_Late) Values (@S_UID, @C_DeviceID, @In_Time, @Period, @Late)
												commit
											end
										else 
											begin
												print 'S_UID: null'
												rollback
											end
									end try
									begin catch
										rollback
									end catch
							set @value = @value + 1
						end
					End
				begin tran
					drop table #Data_Dump_Temp
				commit
			end	
		END