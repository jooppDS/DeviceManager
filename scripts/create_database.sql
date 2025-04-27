-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagerDB')
BEGIN
    CREATE DATABASE DeviceManagerDB;
END
GO

USE DeviceManagerDB;
GO

-- Create Device table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Device')
BEGIN
    CREATE TABLE Device (
        Id NVARCHAR(50) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        IsEnabled BIT NOT NULL
    );
END
GO

-- Create Embedded table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Embedded')
BEGIN
    CREATE TABLE Embedded (
        Id INT PRIMARY KEY IDENTITY(1,1),
        IPAddress VARCHAR(15) NOT NULL,
        NetworkName VARCHAR(100) NOT NULL,
        DeviceId NVARCHAR(50) UNIQUE NOT NULL,
        FOREIGN KEY (DeviceId) REFERENCES Device(Id)
    );
END
GO

-- Create PersonalComputer table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PersonalComputer')
BEGIN
    CREATE TABLE PersonalComputer (
        Id INT PRIMARY KEY IDENTITY(1,1),
        OperationSystem VARCHAR(100),
        DeviceId NVARCHAR(50) UNIQUE NOT NULL,
        FOREIGN KEY (DeviceId) REFERENCES Device(Id)
    );
END
GO

-- Create Smartwatch table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Smartwatch')
BEGIN
    CREATE TABLE Smartwatch (
        Id INT PRIMARY KEY IDENTITY(1,1),
        BatteryPercentage INT NOT NULL,
        DeviceId NVARCHAR(50) UNIQUE NOT NULL,
        FOREIGN KEY (DeviceId) REFERENCES Device(Id)
    );
END
GO 