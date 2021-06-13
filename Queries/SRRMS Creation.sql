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

CREATE TABLE SRRMS_DB.dbo.Student_Info(
	SI_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	S_UID uniqueidentifier,
	FOREIGN KEY (S_UID) REFERENCES Student_RFID(S_UID),
	F_Name varchar(100) NOT NULL,
	l_Name varchar(100) NOT NULL,
	HR_Code varchar(10) NOT NULL,
	Y_Level tinyint NOT NULL,
	Subject_List varchar(max)
)
GO

CREATE TABLE SRRMS_DB.dbo.Class_Info(
	C_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	C_Name varchar(80),
	C_Description varchar(255),
	C_DeviceID char(10),
	FOREIGN KEY (C_DeviceID) REFERENCES Devices(C_DeviceID),
)
GO

CREATE TABLE SRRMS_DB.dbo.Times(
	T_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Period_Num tinyint NOT NULL UNIQUE,
	Period_Time time NOT NULL,
	Period_Day varchar(10) NOT NULL,
)
GO

CREATE TABLE SRRMS_DB.dbo.Time_Override(
	TO_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Period_Num tinyint NOT NULL,
	FOREIGN KEY(Period_Num) REFERENCES Times(Period_Num),
	Period_Time time NOT NULL,
	Period_Day date NOT NULL,
)
GO

CREATE TABLE SRRMS_DB.dbo.Locations(
	L_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	C_ID int NOT NULL,
	FOREIGN KEY (C_ID) REFERENCES Class_Info(C_ID),
	Date_Day date NOT NULL,
	Period_Num tinyint NOT NULL,
	FOREIGN KEY(Period_Num) REFERENCES Times(Period_Num),
	D_ID int NOT NULL,
	FOREIGN KEY (D_ID) REFERENCES Devices(D_ID),
)
GO

CREATE TABLE SRRMS_DB.dbo.Location_Override(
	LO_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	C_ID int NOT NULL,
	FOREIGN KEY (C_ID) REFERENCES Class_Info(C_ID),
	Date_Day date NOT NULL,
	Period_Num tinyint NOT NULL,
	FOREIGN KEY(Period_Num) REFERENCES Times(Period_Num),
	D_ID int NOT NULL,
	FOREIGN KEY (D_ID) REFERENCES Devices(D_ID),
)
GO

CREATE TABLE SRRMS_DB.dbo.Attendence(
	A_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	SI_ID int NOT NULL,
	FOREIGN KEY (SI_ID) REFERENCES  Student_Info(SI_ID),
	A_Day date,
	Period_Num tinyint,
	FOREIGN KEY(Period_Num) REFERENCES Times(Period_Num),
	A_Attended bit NOT NULL,
	C_Description varchar(255),
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
