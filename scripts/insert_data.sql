USE DeviceManagerDB;
GO

-- Insert sample devices
-- Device: Gaming PC
IF NOT EXISTS (SELECT * FROM Device WHERE Id = 'PC-001')
BEGIN
    INSERT INTO Device (Id, Name, IsEnabled)
    VALUES ('PC-001', 'Gaming PC', 1);
    
    INSERT INTO PersonalComputer (OperationSystem, DeviceId)
    VALUES ('Windows 11', 'PC-001');
END
GO

-- Device: Security Camera (Embedded)
IF NOT EXISTS (SELECT * FROM Device WHERE Id = 'EMB-001')
BEGIN
    INSERT INTO Device (Id, Name, IsEnabled)
    VALUES ('EMB-001', 'Security Camera', 1);
    
    INSERT INTO Embedded (IPAddress, NetworkName, DeviceId)
    VALUES ('192.168.1.100', 'SecurityNetwork', 'EMB-001');
END
GO

-- Device: Fitness Watch (Smartwatch)
IF NOT EXISTS (SELECT * FROM Device WHERE Id = 'SW-001')
BEGIN
    INSERT INTO Device (Id, Name, IsEnabled)
    VALUES ('SW-001', 'Fitness Watch', 1);
    
    INSERT INTO Smartwatch (BatteryPercentage, DeviceId)
    VALUES (85, 'SW-001');
END
GO 