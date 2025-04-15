using System;

namespace task2
{
    /// <summary>
    /// Represents a general device with basic information and functionality.
    /// </summary>
    public abstract class Device
    {
        /// <summary>
        /// Internal unique device identifier.
        /// </summary>
        private string _id;

        /// <summary>
        /// The device name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Indicates whether the device is powered on (active) or off.
        /// </summary>
        private bool _active;

        protected Device() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="id">Unique identifier for the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is initially active.</param>
        protected Device(string id, string name, bool active)
        {
            _id = id;
            _name = name;
            _active = active;
        }

        /// <summary>
        /// Gets or sets the device's name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the device's unique identifier.
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the device is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Returns a file-ready format of the device data.
        /// </summary>
        /// <returns>A string representing the device data suitable for file saving.</returns>
        public virtual string GetFileFormat()
        {
            return $"{_name},{_active}";
        }

        /// <summary>
        /// Powers off the device.
        /// </summary>
        /// <returns><c>true</c> if the device is successfully powered off; otherwise, <c>false</c>.</returns>
        public virtual bool PowerOff()
        {
            _active = false;
            return true;
        }

        /// <summary>
        /// Powers on the device.
        /// </summary>
        /// <returns><c>true</c> if the device is successfully powered on; otherwise, <c>false</c>.</returns>
        public virtual bool PowerOn()
        {
            _active = true;
            return true;
        }

        /// <summary>
        /// Edits this device using data from another device.
        /// </summary>
        /// <param name="otherDevice">Another <see cref="Device"/> object that holds the new data.</param>
        /// <returns><c>true</c> if successfully edited, otherwise <c>false</c>.</returns>
        public abstract bool Edit(Device otherDevice);

        /// <summary>
        /// Returns a string that represents the current device.
        /// </summary>
        /// <returns>A string containing device information.</returns>
        public override string ToString()
        {
            return $"ID: {_id}\nName: {_name}\nActive: {_active}\n";
        }
    }
}
