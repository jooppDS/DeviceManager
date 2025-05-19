-- Insert into Device table
INSERT INTO Device (Id, Name, IsEnabled)
VALUES
    ('device1', 'Embedded Device 1', 1),
    ('device2', 'Personal Computer 1', 1),
    ('device3', 'Smartwatch 1', 1);

-- Insert into EmbeddedDevice table
INSERT INTO EmbeddedDevice (Id, Ip, NetworkName)
VALUES
    ('device1', '192.168.0.10', 'Network1');

-- Insert into PersonalComputer table
INSERT INTO PersonalComputer (Id, OS)
VALUES
    ('device2', 'Windows 10');

-- Insert into Smartwatch table
INSERT INTO Smartwatch (Id, Power)
VALUES
    ('device3', 85);