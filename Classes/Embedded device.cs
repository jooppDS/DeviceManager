using System.Text.RegularExpressions;

namespace task2
{
    /// <summary>
    /// Represents an embedded device that is part of a specific network.
    /// </summary>
    class EmbededDevice : Device
    {
       
        private const string manType = "ED";

        private string Ip;

        private string networkName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbededDevice"/> class.
        /// </summary>
        /// <param name="id">Unique identifier of the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is active.</param>
        /// <param name="ip">The IP address of the device.</param>
        /// <param name="networkname">The network name the device is connected to.</param>
        public EmbededDevice(string id, string name, bool active, string ip, string networkname)
            : base(id, name, active)
        {
            try
            {
                _ip = ip;
                _networkname = networkname;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Attempts to connect the device to the network. Throws an exception if the network name is invalid.
        /// </summary>
        /// <exception cref="ConnectionException">Thrown when the network name does not match the expected pattern.</exception>
        private void Connect()
        {
            Regex rg = new Regex("MD Ltd.”");
            if (!rg.IsMatch(networkName))
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
            }
            catch (ConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }

            _active = true;
            return _active;
        }

        /// <summary>
        /// Edits this embedded device using the properties of another <see cref="Device"/>.
        /// </summary>
        /// <param name="otherDevice">A <see cref="Device"/> that should be an <see cref="EmbededDevice"/>.</param>
        /// <returns><c>true</c> if edit is successful; otherwise throws an exception.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="otherDevice"/> is not an <see cref="EmbededDevice"/>.</exception>
        public override bool Edit(Device otherDevice)
        {
            if (otherDevice is not EmbededDevice newED)
                throw new ArgumentException();

            _id = newED._id;
            _name = newED._name;
            _active = newED._active;
            _ip = newED._ip;
            _networkname = newED._networkname;

            return true;
        }

        /// <summary>
        /// Gets or sets the network name of the device.
        /// </summary>
        /// <exception cref="ConnectionException">Thrown if the network name does not match the required pattern.</exception>
        public string _networkname
        {
            get { return networkName; }
            set
            {
                Regex rg = new Regex("MD Ltd.");
                if (rg.IsMatch(value))
                    networkName = value;
                else
                    throw new ConnectionException();
            }
        }

        /// <summary>
        /// Gets or sets the IP address of the device.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the IP address does not match the required pattern.</exception>
        public string _ip
        {
            get { return Ip; }
            set
            {
                Regex rg = new Regex("([0-9]+.*)+");
                if (rg.IsMatch(value))
                    Ip = value;
                else
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns a file-ready format of this embedded device data.
        /// </summary>
        /// <returns>A string representing the embedded device data suitable for file saving.</returns>
        public override string getFileFormat()
        {
            return manType + ',' + _name + ',' + Ip + ',' + networkName;
        }

        /// <summary>
        /// Returns a string that represents this <see cref="EmbededDevice"/>.
        /// </summary>
        /// <returns>A string containing this device's information.</returns>
        public override string ToString()
        {
            return base.ToString() + "IP: " + _ip + "\n" + "NET: " + _networkname + "\n";
        }
    }
}
