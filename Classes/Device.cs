using System.Linq.Expressions;

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
        private string Id { set; get; }
        
        /// <summary>
        /// The device name.
        /// </summary>
        private string Name { set; get; }
        
        /// <summary>
        /// Indicates whether the device is powered on (active) or off.
        /// </summary>
        private bool Active;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="id">Unique identifier for the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is initially active.</param>
        protected Device(string id, string name, bool active)
        {
            Id = id;
            Name = name;
            Active = active;
        }

        /// <summary>
        /// Gets or sets the device's name.
        /// </summary>
        public string _name
        {
            get { return Name; }
            set { Name = value; }
        }

        /// <summary>
        /// Gets or sets the device's unique identifier.
        /// </summary>
        public string _id
        {
            get { return Id; }
            set { Id = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the device is currently active.
        /// </summary>
        public bool _active
        {
            get { return Active; }
            set { Active = value; }
        }

        /// <summary>
        /// Returns a file-ready format of the device data.
        /// </summary>
        /// <returns>A string representing the device data suitable for file saving.</returns>
        public virtual string getFileFormat()
        {
            return Name + ',' + Active;
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
            return "ID: " + Id + "\nName: " + Name + "\nActive: " + Active + "\n";
        }
    }
}
