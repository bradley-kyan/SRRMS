CREATE DATABASE SRRMS_DB
GO

CREATE TABLE SRRMS_DB.dbo.Devices(
	D_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Class_Description varchar(50),
	C_DeviceID char(10) UNIQUE,
)
GO

CREATE TABLE SRRMS_DB.dbo.Class_Info(
	CI_ID int NOT NULL IDENTITY(1,1),
	C_UID uniqueidentifier not null default newid() Primary key,
	D_ID int not null,
	FOREIGN KEY (D_ID) REFERENCES Devices(D_ID),
	C_Name varchar(80),
	C_Description varchar(255),
	C_DeviceID char(10),
	FOREIGN KEY (C_DeviceID) REFERENCES Devices(C_DeviceID),
	C_Override bit not null,
	C_Override_Start date,
	C_Override_End date,
	C_Day date not null,
	C_Period tinyint not null
)
GO


CREATE TABLE SRRMS_DB.dbo.Student_RFID(
	SR_ID int NOT NULL IDENTITY(1,1),
	S_UID uniqueidentifier NOT NULL default newid() PRIMARY KEY,
	Card_ID varchar(255) UNIQUE
)
GO

CREATE TABLE SRRMS_DB.dbo.Student_Info(
	SI_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	S_UID uniqueidentifier,
	FOREIGN KEY (S_UID) REFERENCES Student_RFID(S_UID),
	F_Name varchar(100) NOT NULL,
	L_Name varchar(100) NOT NULL,
	HR_Code char(5) NOT NULL,
	Y_Level tinyint NOT NULL,
	Subject_List varchar(max)
)
GO

CREATE TABLE SRRMS_DB.dbo.Times(
	T_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	T_Day date not null,
	Period_Time1 time,
	Period_End1 time,
	Period_Time2 time,
	Period_End2 time,
	Period_Time3 time,
	Period_End3 time,
	Period_Time4 time,
	Period_End4 time,
	Period_Time5 time,
	Period_End5 time,
)
GO

CREATE TABLE SRRMS_DB.dbo.Time_Override(
	T_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	T_Day date not null,
	Period_Time1 time,
	Period_End1 time,
	Period_Time2 time,
	Period_End2 time,
	Period_Time3 time,
	Period_End3 time,
	Period_Time4 time,
	Period_End4 time,
	Period_Time5 time,
	Period_End5 time,
)
GO

CREATE TABLE SRRMS_DB.dbo.Attendence(
	A_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	S_UID uniqueidentifier NOT NULL,
	FOREIGN KEY (S_UID) REFERENCES  Student_RFID(S_UID),
	A_Date date default cast( getdate() as date) not null,
	Real_TimeIn datetime not null,
	A_Period tinyint not null,
	A_Late bit not null,
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

/*drop database SRRMS_DB*/