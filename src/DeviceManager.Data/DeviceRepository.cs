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
                var command = new SqlCommand("SELECT Id, Name, IsEnabled, Version FROM Device", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        devices.Add(new Device
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            IsEnabled = reader.GetBoolean(2),
                            Version = (byte[])reader.GetValue(3)
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
                var command = new SqlCommand("SELECT Id, Name, IsEnabled, Version FROM Device WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;
                    var device = new Device
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        IsEnabled = reader.GetBoolean(2),
                        Version = (byte[])reader.GetValue(3)
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
                                OS = pcReader.GetString(1),
                                Version = device.Version
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
                                NetworkName = embReader.GetString(2),
                                Version = device.Version
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
                                Power = swReader.GetInt64(1),
                                Version = device.Version
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

            if (device.Version == null)
                throw new ArgumentException("Version is required for optimistic concurrency");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    if (details is PersonalComputer pc)
                    {
                        using var cmd = new SqlCommand("UpdatePersonalComputer", connection) {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@Id", device.Id);
                        cmd.Parameters.AddWithValue("@Name", device.Name);
                        cmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        cmd.Parameters.AddWithValue("@OS", pc.OS);
                        cmd.Parameters.AddWithValue("@Version", device.Version);
                        cmd.ExecuteNonQuery();
                    }
                    else if (details is EmbeddedDevice emb)
                    {
                        using var cmd = new SqlCommand("UpdateEmbeddedDevice", connection) {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@Id", device.Id);
                        cmd.Parameters.AddWithValue("@Name", device.Name);
                        cmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        cmd.Parameters.AddWithValue("@Ip", emb.Ip);
                        cmd.Parameters.AddWithValue("@NetworkName", emb.NetworkName);
                        cmd.Parameters.AddWithValue("@Version", device.Version);
                        cmd.ExecuteNonQuery();
                    }
                    else if (details is Smartwatch sw)
                    {
                        using var cmd = new SqlCommand("UpdateSmartwatch", connection) {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@Id", device.Id);
                        cmd.Parameters.AddWithValue("@Name", device.Name);
                        cmd.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                        cmd.Parameters.AddWithValue("@Power", sw.Power);
                        cmd.Parameters.AddWithValue("@Version", device.Version);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex) when (ex.Number == 50000) 
                {
                    throw new ConcurrencyException("The record has been modified by another user.", ex);
                }
            }
        }

        public void DeleteDevice(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Device WHERE Id = @Id", connection);
                checkCmd.Parameters.AddWithValue("@Id", id);
                int count = (int)checkCmd.ExecuteScalar();
                
                if (count == 0)
                {
                    throw new KeyNotFoundException($"Device with ID {id} not found.");
                }

                var transaction = connection.BeginTransaction();
                try
                {
                    var cmd = new SqlCommand("DELETE FROM Device WHERE Id = @Id", connection, transaction);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        transaction.Rollback();
                        throw new KeyNotFoundException($"Device with ID {id} not found.");
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
    }

    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 