using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DeviceManagerLib;

namespace DeviceManager.Data
{
    public class DeviceRepository
    {
        private readonly string _connectionString;

        public DeviceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Methods for CRUD will be added here

        public IEnumerable<Device> GetAllDevices()
        {
            var devices = new List<Device>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, IsEnabled FROM Device", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        devices.Add(new Device
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            IsEnabled = reader.GetBoolean(2)
                        });
                    }
                }
            }
            return devices;
        }

        public object GetDeviceById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Name, IsEnabled FROM Device WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;
                    var device = new Device
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        IsEnabled = reader.GetBoolean(2)
                    };
                    reader.Close();

                    // Check for PersonalComputer
                    command = new SqlCommand("SELECT Id, OS FROM PersonalComputer WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var pcReader = command.ExecuteReader())
                    {
                        if (pcReader.Read())
                        {
                            return new PersonalComputer
                            {
                                Id = pcReader.GetString(0),
                                OS = pcReader.GetString(1)
                            };
                        }
                    }

                    // Check for EmbeddedDevice
                    command = new SqlCommand("SELECT Id, Ip, NetworkName FROM EmbeddedDevice WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var embReader = command.ExecuteReader())
                    {
                        if (embReader.Read())
                        {
                            return new EmbeddedDevice
                            {
                                Id = embReader.GetString(0),
                                Ip = embReader.GetString(1),
                                NetworkName = embReader.GetString(2)
                            };
                        }
                    }

                    // Check for Smartwatch
                    command = new SqlCommand("SELECT Id, Power FROM Smartwatch WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    using (var swReader = command.ExecuteReader())
                    {
                        if (swReader.Read())
                        {
                            return new Smartwatch
                            {
                                Id = swReader.GetString(0),
                                Power = swReader.GetInt64(1)
                            };
                        }
                    }

                    
                    return device;
                }
            }
        }

        public void CreateDevice(Device device, object details)
        {
            if (string.IsNullOrWhiteSpace(device.Id) || string.IsNullOrWhiteSpace(device.Name))
                throw new ArgumentException("Invalid device data");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                  
                    if (details is PersonalComputer pc)
                    {
                        using var pcCmd = new SqlCommand("AddPersonalComputer", connection, transaction) {
                            CommandType = CommandType.StoredProcedure
                        };
                        pcCmd.Parameters.AddWithValue("@DeviceId", device.Id);
                        pcCmd.Parameters.AddWithValue("@Name", device.Name);
                        pcCmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        pcCmd.Parameters.AddWithValue("@OperationSystem", pc.OS);
                        pcCmd.ExecuteNonQuery();
                    }
                    else if (details is EmbeddedDevice emb)
                    {
                        using var embCmd = new SqlCommand("AddEmbedded", connection, transaction) {
                            CommandType = CommandType.StoredProcedure
                        };
                        embCmd.Parameters.AddWithValue("@DeviceId", device.Id);
                        embCmd.Parameters.AddWithValue("@Name", device.Name);
                        embCmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        embCmd.Parameters.AddWithValue("@IpAddress", emb.Ip);
                        embCmd.Parameters.AddWithValue("@NetworkName", emb.NetworkName);
                        embCmd.ExecuteNonQuery();
                    }
                    else if (details is Smartwatch sw)
                    {
                        using var swCmd = new SqlCommand("AddSmartwatch", connection, transaction) {
                            CommandType = CommandType.StoredProcedure
                        };
                        swCmd.Parameters.AddWithValue("@DeviceId", device.Id);
                        swCmd.Parameters.AddWithValue("@Name", device.Name);
                        swCmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        swCmd.Parameters.AddWithValue("@BatteryPercentage", sw.Power);
                        swCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateDevice(Device device, object details)
        {
            if (string.IsNullOrWhiteSpace(device.Id) || string.IsNullOrWhiteSpace(device.Name))
                throw new ArgumentException("Invalid device data");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    var cmd = new SqlCommand("UPDATE Device SET Name = @Name, IsEnabled = @IsEnabled WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", device.Id);
                    cmd.Parameters.AddWithValue("@Name", device.Name);
                    cmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                    cmd.ExecuteNonQuery();

                    if (details is PersonalComputer pc)
                    {
                        var pcCmd = new SqlCommand("UPDATE PersonalComputer SET OS = @OS WHERE Id = @Id", connection, transaction);
                        pcCmd.Parameters.AddWithValue("@Id", pc.Id);
                        pcCmd.Parameters.AddWithValue("@OS", pc.OS);
                        pcCmd.ExecuteNonQuery();
                    }
                    else if (details is EmbeddedDevice emb)
                    {
                        var embCmd = new SqlCommand("UPDATE EmbeddedDevice SET Ip = @Ip, NetworkName = @NetworkName WHERE Id = @Id", connection, transaction);
                        embCmd.Parameters.AddWithValue("@Id", emb.Id);
                        embCmd.Parameters.AddWithValue("@Ip", emb.Ip);
                        embCmd.Parameters.AddWithValue("@NetworkName", emb.NetworkName);
                        embCmd.ExecuteNonQuery();
                    }
                    else if (details is Smartwatch sw)
                    {
                        var swCmd = new SqlCommand("UPDATE Smartwatch SET Power = @Power WHERE Id = @Id", connection, transaction);
                        swCmd.Parameters.AddWithValue("@Id", sw.Id);
                        swCmd.Parameters.AddWithValue("@Power", sw.Power);
                        swCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteDevice(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    var cmd = new SqlCommand("DELETE FROM PersonalComputer WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM EmbeddedDevice WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM Smartwatch WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM Device WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
} 