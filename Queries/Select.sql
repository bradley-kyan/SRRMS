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