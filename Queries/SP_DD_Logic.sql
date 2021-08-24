Create Proc ReseedDD
As
Begin
	Begin transaction
		begin try
			Delete from SRRMS_DB.dbo.Data_Dump
			DBCC CHECKIDENT ('Data_Dump', RESEED, 0)
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
	Declare @LateValueInMin int = 10
	Declare @getdate char(9)
	declare @now datetime = getdate()
	Exec StringToDate @now, @getdate OUTPUT 
	Declare @override char(9) = (select T_Day from SRRMS_DB.dbo.Time_Override where T_Day = @getdate)

	if @override is null
		begin
			if @TimeInput between (select Period_Time1 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End1 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time1 from SRRMS_DB.dbo.Times where T_Day = @getdate))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				end
			else if @TimeInput between (select Period_Time2 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End2 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time2 from SRRMS_DB.dbo.Times where T_Day = @getdate))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				end
			else if @TimeInput between (select Period_Time3 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End3 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time3 from SRRMS_DB.dbo.Times where T_Day = @getdate))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				end
			else if @TimeInput between (select Period_Time4 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End4 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time4 from SRRMS_DB.dbo.Times where T_Day = @getdate))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				end
			else if @TimeInput between (select Period_Time5 from SRRMS_DB.dbo.Times where T_Day = @getdate) AND (select Period_End5 from SRRMS_DB.dbo.Times where T_Day = @getdate)
				begin
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time5 from SRRMS_DB.dbo.Times where T_Day = @getdate))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				end
			else	
				return
			return
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
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				else if @TimeInput between (select Period_Time2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time2 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				else if @TimeInput between (select Period_Time3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time3 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				else if @TimeInput between (select Period_Time4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time4 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
					end
				else if @TimeInput between (select Period_Time5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override) AND (select Period_End5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override)
					if @TimeInput > dateadd(mi, @LateValueInMin,(select Period_Time5 from SRRMS_DB.dbo.Time_Override where T_OverrideBegin = @override))
					begin
						select @PeriodOutput = 1
						select @Late = 1
					end
					else
					begin
						select @PeriodOutput = 1
						select @Late = 0
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
	Declare @lastEditTime datetime = (
		SELECT last_user_update
		FROM   sys.dm_db_index_usage_stats us
			   JOIN sys.tables t
					 ON t.object_id = us.object_id
		WHERE  database_id = db_id()
			   AND t.object_id = object_id('dbo.Data_Dump') 
	)

	declare @dateDiff int = (
		select DATEDIFF(s, @lastEditTime, GETDATE())
	)
	declare @DD varchar = (Select DD_ID From SRRMS_DB.dbo.Data_Dump where DD_ID = '1')
	Declare @DbAccessMin int = 5
	IF @dateDiff >= @DbAccessMin
		BEGIN
			IF @DD is null
			begin
				Print 'Data_Dump table is empty'
				rollback
				return
			end
			else
			begin
				SELECT * into #Data_Dump_Temp from SRRMS_DB.dbo.Data_Dump
				Exec ReseedDD

				Declare @value int = 0
					While (1 = 1)
					Begin
						select top(1) @value = DD_ID from SRRMS_DB.dbo.Data_Dump where DD_ID > @value order by DD_ID
						if @@ROWCOUNT = 0 break;
						begin
						begin tran
							begin try
								Declare @In_Time datetime = (select In_Time from #Data_Dump_Temp where DD_ID = @value)
								Declare @C_DeviceID datetime = (select C_DeviceID from #Data_Dump_Temp where DD_ID = @value)
								Declare @Period tinyint
								Declare @Late bit
								Exec GetPeriod @In_Time, @Period OUTPUT, @Late OUTPUT
								Declare @S_UID uniqueidentifier = (select S_UID from SRRMS_DB.dbo.Student_RFID where Card_ID = (select Card_ID from #Data_Dump_Temp where DD_ID = @value))
								if @S_UID is not null
									insert into SRRMS_DB.dbo.Attendence (S_UID, C_DeviceID, Real_TimeIn, A_Period, A_Late) Values (@S_UID, @C_DeviceID, @In_Time, @Period, @Late)
								commit
							end try
							begin catch
								rollback
							end catch
						end
					End
				begin tran
					Drop table #Data_Dump_Temp
				commit
			end	
		END
	ELSE
		BEGIN
			print 'DB has bee accessed less than ' + @DbAccessMin + ' second(s) ago'
		END 
END
GO