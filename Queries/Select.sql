/****** Script for SelectTopNRows command from SSMS  ******/
-- exec DD_Logic
SELECT TOP (1000) [S_UID]
      ,[Card_ID]
  FROM [SRRMS_DB].[dbo].[Student_RFID]

  go
  SELECT TOP (1000) [SI_ID]
      ,[S_UID]
      ,[F_Name]
      ,[L_Name]
      ,[HR_Code]
      ,[Y_Level]
      ,[Subject_List]
  FROM [SRRMS_DB].[dbo].[Student_Info]

  SELECT TOP (1000) [D_ID]
      ,[Class_Description]
      ,[C_DeviceID]
  FROM [SRRMS_DB].[dbo].[Devices]
  go


 Select * from [SRRMS_DB].[dbo].[Class_Info] order by [C_Override] ASC, [C_Name] DESC

 Select * from [SRRMS_DB].[dbo].[Attendence]

 Select * from [SRRMS_DB].[dbo].[Times]

 Select * from [SRRMS_DB].[dbo].[Time_Override]

 Select * from [SRRMS_DB].[dbo].[Data_Dump]

 /*

 Declare @time time = getdate()
 declare @date datetime = getdate()
 Declare @p1 time = DateAdd(mi, 0, @time)
 Declare @p1e time = DateAdd(mi, 10, @time)
 Declare @p2 time = DateAdd(mi, 11, @time)
 Declare @p2e time = DateAdd(mi, 20, @time)
 Declare @p3 time = DateAdd(mi, 21, @time)
 Declare @p3e time = DateAdd(mi, 30, @time)
 Declare @p4 time = DateAdd(mi, 31, @time)
 Declare @p4e time = DateAdd(mi, 40, @time)
 Declare @p5 time = DateAdd(mi, 41, @time)
 Declare @p5e time = DateAdd(mi, 50, @time)
 exec TimesAdd 'thursday', @p1, @p1e, @p2, @p2e, @p3, @p3e, @p4, @p4e, @p5, @p5e

  Select * from [SRRMS_DB].[dbo].[Times]

 */

 -- Drop table #Data_Dump_Temp
 /*
 Declare @date varchar(255) = getdate()
 declare @tryconvert char(9)
 begin try
		Set @tryconvert = datename(dw, convert(date, @date))
		end try
		begin catch
		set @tryconvert = @date
		end catch

		print @tryconvert

*/
/*

delete SRRMS_DB.dbo.[Attendence]
DBCC CHECKIDENT ('SRRMS_DB.dbo.Attendence', RESEED, 0)

delete SRRMS_DB.dbo.times
DBCC CHECKIDENT ('SRRMS_DB.dbo.times', RESEED, 0)

*/

