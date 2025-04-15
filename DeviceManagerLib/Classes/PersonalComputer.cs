using System;

namespace task2
{
    /// <summary>
    /// Represents a Personal Computer device with an optional operating system.
    /// </summary>
    public class PersonalComputer : Device
    {
        private const string ManType = "P";

        private string _os;

        public PersonalComputer() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalComputer"/> class with an OS.
        /// </summary>
        /// <param name="id">Unique identifier of the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is active.</param>
        /// <param name="os">The operating system of the PC.</param>
        public PersonalComputer(string id, string name, bool active, string os)
            : base(id, name, active)
        {
            _os = os;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalComputer"/> class without specifying an OS.
        /// </summary>
        /// <param name="id">Unique identifier of the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is active.</param>
        public PersonalComputer(string id, string name, bool active)
            : base(id, name, active)
        {
        }

        /// <summary>
        /// Powers on the PC. Throws an exception if the OS is not defined.
        /// </summary>
        /// <returns><c>true</c> if successfully powered on; otherwise, <c>false</c>.</returns>
        /// <exception cref="EmptySystemException">Thrown if the OS is null or empty.</exception>
        public override bool PowerOn()
        {
            bool success = !string.IsNullOrWhiteSpace(_os);
            if (!success)
            {
                throw new EmptySystemException();
            }

            IsActive = success;
            return success;
        }

        /// <summary>
        /// Edits this PC using the properties of another <see cref="Device"/>.
        /// </summary>
        /// <param name="otherDevice">A <see cref="Device"/> that should be a <see cref="PersonalComputer"/>.</param>
        /// <returns><c>true</c> if edit is successful; otherwise throws an exception.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="otherDevice"/> is not a <see cref="PersonalComputer"/>.</exception>
        public override bool Edit(Device otherDevice)
        {
            if (otherDevice is not PersonalComputer newComputer)
                throw new ArgumentException();
            
            Name = newComputer.Name;
            IsActive = newComputer.IsActive;
            OS = newComputer.OS;

            return true;
        }

        /// <summary>
        /// Gets or sets the operating system of this PC.
        /// </summary>
        public string OS
        {
            get { return _os; }
            set { _os = value; }
        }

        /// <summary>
        /// Returns a file-ready format of this PC's data.
        /// </summary>
        /// <returns>A string representing the PC data suitable for file saving.</returns>
        public override string GetFileFormat()
        {
            return string.IsNullOrWhiteSpace(_os)
                ? $"{ManType},{base.GetFileFormat()}"
                : $"{ManType},{base.GetFileFormat()},{_os}";
        }

        /// <summary>
        /// Returns a string that represents this <see cref="PersonalComputer"/>.
        /// </summary>
        /// <returns>A string containing this device's information.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}OS: {_os}\n";
        }
    }
}
