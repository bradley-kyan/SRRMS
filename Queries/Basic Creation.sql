CREATE DATABASE SRRMS_DB
GO

CREATE TABLE SRRMS_DB.dbo.Devices(
	D_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Class_Description varchar(50),
	C_DeviceID char(10) UNIQUE,
)
GO

CREATE TABLE SRRMS_DB.dbo.Student_RFID(
	S_UID uniqueidentifier NOT NULL default newid() PRIMARY KEY,
	Card_ID varchar(255) UNIQUE
)
GO

CREATE TABLE SRRMS_DB.dbo.Data_Dump(
	DD_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	C_DeviceID char(10),
	FOREIGN KEY(C_DeviceID) REFERENCES Devices(C_DeviceID),
	Card_ID varchar(255),
	FOREIGN KEY(Card_ID) REFERENCES Student_RFID(Card_ID),
	In_Time datetime NOT NULL
)
