CREATE TYPE [dbo].[MyTableType] AS TABLE(
    [C_DeviceID] [varchar](max) NULL,
	[Card_ID] [varchar](max) NULL,
	[In_Time] [datetime] NULL
)
GO
CREATE PROCEDURE [dbo].[InsertTable]
    @myTableType MyTableType readonly
AS
BEGIN
	delete from [SRRMS_DB].[dbo].[Data_Dump];
	WAITFOR DELAY '0:0:5';
    insert into [SRRMS_DB].[dbo].[Data_Dump] select * from @myTableType
END

