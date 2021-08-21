--Create procedure DD_Logic
--AS
--BEGIN

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
		select DATEDIFF(mi, @lastEditTime, GETDATE())
	)
	declare @DD varchar = (Select DD_ID From Data_Dump where DD_ID = '1')

		IF @dateDiff >= 1
			BEGIN
				IF @DD is null
				begin
					SELECT 'null' AS 'Status'
				end
				else
				begin
					SELECT * into #Data_Dump_Temp from Data_Dump
					Delete from Data_Dump
					DBCC CHECKIDENT ('Data_Dump', RESEED, 0)
					SELECT 'reseeded' AS 'Status'
						SELECT * from #Data_Dump_Temp order by Card_ID, In_Time;
					--Drop table #Data_Dump_Temp
				end	
			END
		ELSE
			BEGIN
				SELECT 'no' AS 'Status'
			END 
			

--END

--DROP PROCEDURE DD_Logic
