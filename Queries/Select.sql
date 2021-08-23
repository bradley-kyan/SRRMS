/****** Script for SelectTopNRows command from SSMS  ******/
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
  go
  SELECT TOP (1000) [D_ID]
      ,[Class_Description]
      ,[C_DeviceID]
  FROM [SRRMS_DB].[dbo].[Devices]
  go

 Select * from [SRRMS_DB].[dbo].[Class_Info] order by [C_Override] ASC, [C_Name] DESC

 Select * from [SRRMS_DB].[dbo].[Attendence]

 Select * from [SRRMS_DB].[dbo].[Times]

 Select * from [SRRMS_DB].[dbo].[Time_Override]