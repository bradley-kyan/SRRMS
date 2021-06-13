USE SRRMS_DB

CREATE TYPE [dbo].[MyTableType] AS TABLE(
    [C_DeviceID] [char](10) NULL,
	[Card_ID] [varchar](255) NULL,
	[In_Time] [datetime] NULL
)
GO

--DROP PROCEDURE InsertTable
--DROP TYPE MyTableType


CREATE PROC [dbo].[InsertTable](@Table MyTableType READONLY)
AS
BEGIN
    insert into Data_Dump (C_DeviceID,Card_ID,In_Time) select * from @Table
END
