-- Create Device table
CREATE TABLE Device (
                        Id NVARCHAR(50) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        IsEnabled BIT NOT NULL,
                        Version ROWVERSION NOT NULL
);
GO

    -- Create EmbeddedDevice table
CREATE TABLE EmbeddedDevice (
                                Id NVARCHAR(50) PRIMARY KEY,
                                Ip NVARCHAR(15) NOT NULL,
                                NetworkName NVARCHAR(100) NOT NULL,
                                FOREIGN KEY (Id) REFERENCES Device(Id) ON DELETE CASCADE
);
GO

    -- Create PersonalComputer table
CREATE TABLE PersonalComputer (
                                  Id NVARCHAR(50) PRIMARY KEY,
                                  OS NVARCHAR(100) NOT NULL,
                                  FOREIGN KEY (Id) REFERENCES Device(Id) ON DELETE CASCADE
);
GO

    -- Create Smartwatch table
CREATE TABLE Smartwatch (
                            Id NVARCHAR(50) PRIMARY KEY,
                            Power BIGINT NOT NULL,
                            FOREIGN KEY (Id) REFERENCES Device(Id) ON DELETE CASCADE
);
GO
