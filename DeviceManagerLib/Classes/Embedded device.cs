using System.Text.RegularExpressions;

namespace task2
{
    /// <summary>
    /// Represents an embedded device that is part of a specific network.
    /// </summary>
    public class EmbededDevice : Device
    {
        private const string ManType = "ED";

        private string _ip;
        private string _networkName;

        public EmbededDevice() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbededDevice"/> class.
        /// </summary>
        /// <param name="id">Unique identifier of the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is active.</param>
        /// <param name="ip">The IP address of the device.</param>
        /// <param name="networkName">The network name the device is connected to.</param>
        public EmbededDevice(string id, string name, bool active, string ip, string networkName)
            : base(id, name, active)
        {
            this.Ip = ip;
            this.NetworkName = networkName;
        }

        /// <summary>
        /// Attempts to connect the device to the network. Throws an exception if the network name is invalid.
        /// </summary>
        /// <exception cref="ConnectionException">Thrown when the network name does not match the expected pattern.</exception>
        private void Connect()
        {
            Regex rg = new(@"MD Ltd\.");
            if (!rg.IsMatch(_networkName))
                throw new ConnectionException();
        }

        /// <summary>
        /// Powers on the device and attempts a network connection.
        /// </summary>
        /// <returns><c>true</c> if the device is successfully powered on; otherwise <c>false</c>.</returns>
        public override bool PowerOn()
        {
            try
            {
                Connect();
                IsActive = true;
            }
            catch (ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
                IsActive = false;
            }

            return IsActive;
        }

        /// <summary>
        /// Edits this embedded device using the properties of another <see cref="Device"/>.
        /// </summary>
        /// <param name="otherDevice">A <see cref="Device"/> that should be an <see cref="EmbededDevice"/>.</param>
        /// <returns><c>true</c> if edit is successful; otherwise throws an exception.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="otherDevice"/> is not an <see cref="EmbededDevice"/>.</exception>
        public override bool Edit(Device otherDevice)
        {
            if (otherDevice is not EmbededDevice newDevice)
                throw new ArgumentException();
            
            Name = newDevice.Name;
            IsActive = newDevice.IsActive;
            Ip = newDevice.Ip;
            NetworkName = newDevice.NetworkName;

            return true;
        }

        /// <summary>
        /// Gets or sets the network name of the device.
        /// </summary>
        /// <exception cref="ConnectionException">Thrown if the network name does not match the required pattern.</exception>
        public string NetworkName
        {
            get => _networkName;
            set
            {
                Regex rg = new(@"MD Ltd\.");
                if (!rg.IsMatch(value))
                    throw new ConnectionException();
                _networkName = value;
            }
        }

        /// <summary>
        /// Gets or sets the IP address of the device.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the IP address does not match the required pattern.</exception>
        public string Ip
        {
            get => _ip;
            set
            {
                Regex rg = new Regex("([0-9]+.*)+");
                if (!rg.IsMatch(value))
                    throw new ArgumentException();
                _ip = value;
            }
        }

        /// <summary>
        /// Returns a file-ready format of this embedded device data.
        /// </summary>
        /// <returns>A string representing the embedded device data suitable for file saving.</returns>
        public override string GetFileFormat()
        {
            return $"{ManType},{Name},{_ip},{_networkName}";
        }

        /// <summary>
        /// Returns a string that represents this <see cref="EmbededDevice"/>.
        /// </summary>
        /// <returns>A string containing this device's information.</returns>
        public override string ToString()
        {
            return base.ToString() + $"IP: {_ip}\nNET: {_networkName}\n";
        }
    }
}
