CREATE TABLE Student_Info(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FOREIGN KEY (S_UID) REFERENCES Student_RFID(S_UID),
	F_Name varchar(100) NOT NULL,
	l_Name varchar(100) NOT NULL,
	HR_Code varchar(10) NOT NULL,
	Y_Level tinyint NOT NULL,
	Subject_List varchar(600)
)
GO
CREATE TABLE Student_RFID(
	S_UID uniqueidentifier NOT NULL default newid() PRIMARY KEY,
	Card_ID varchar(255)
)
GO
CREATE TABLE Class_Info(
	C_UID uniqueidentifier NOT NULL default newid() PRIMARY KEY,
	C_Name varchar(80),
	C_Description varchar(255)
)
GO
CREATE TABLE Location_Override(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FOREIGN KEY (C_UID) REFERENCES Class_Info(C_UID),
	DateDay datetime NOT NULL,
	Period tinyint NOT NULL,
	FOREIGN KEY (ID) REFERENCES Devices(ID),
)
GO
CREATE TABLE Locations(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FOREIGN KEY (C_UID) REFERENCES Class_Info(C_UID),
	DateDay datetime NOT NULL,
	Period tinyint NOT NULL,
	FOREIGN KEY (ID) REFERENCES Devices(ID),
)
GO
CREATE TABLE Devices(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Class varchar(50),
	C_DeviceID varchar(100)
)
GO
CREATE TABLE Attendence(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,

)
GO
CREATE TABLE Times(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,

)
GO
CREATE TABLE Time_Override(
	ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,

)
