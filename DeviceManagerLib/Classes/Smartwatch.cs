using task2.Interfaces;

namespace task2
{
    /// <summary>
    /// Represents a smartwatch device with a battery level.
    /// </summary>
    public class Smartwatch : Device, IPowerNotifier
    {
        private const string ManufacturerType = "SW";
        private long _power;

        public Smartwatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Smartwatch"/> class.
        /// </summary>
        /// <param name="id">Unique identifier of the device.</param>
        /// <param name="name">Name of the device.</param>
        /// <param name="active">Indicates if the device is active.</param>
        /// <param name="power">Battery level of the smartwatch (0-100).</param>
        public Smartwatch(string id, string name, bool active, long power)
            : base(id, name, active)
        {
            _power = power >= 0 && power <= 100 ? power : 0;
        }

        /// <summary>
        /// Powers on the smartwatch if battery is sufficient. Decreases battery by 10% if successful.
        /// </summary>
        /// <returns><c>true</c> if the device is successfully powered on; otherwise, <c>false</c>.</returns>
        /// <exception cref="EmptyBatteryException">Thrown if the battery level is too low to power on.</exception>
        public override bool PowerOn()
        {
            bool success = _power > 11;

            if (success)
            {
                _power -= 10;
            }
            else
            {
                Notify();
                throw new EmptyBatteryException();
            }

            IsActive = success;
            return success;
        }

        /// <summary>
        /// Edits this smartwatch using the properties of another <see cref="Device"/>.
        /// </summary>
        /// <param name="otherDevice">A <see cref="Device"/> that should be a <see cref="Smartwatch"/>.</param>
        /// <returns><c>true</c> if edit is successful; otherwise throws an exception.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="otherDevice"/> is not a <see cref="Smartwatch"/>.</exception>
        public override bool Edit(Device otherDevice)
        {
            if (otherDevice is not Smartwatch newWatch)
            {
                throw new ArgumentException();
            }
            
            Name = newWatch.Name;
            IsActive = newWatch.IsActive;
            Power = newWatch.Power;

            return true;
        }

        /// <summary>
        /// Gets or sets the power level (battery percentage) of this smartwatch.
        /// </summary>
        public long Power
        {
            get { return _power; }
            set { _power = value; }
        }

        /// <summary>
        /// Returns a file-ready format of this smartwatch data.
        /// </summary>
        /// <returns>A string representing the smartwatch data suitable for file saving.</returns>
        public override string GetFileFormat()
        {
            return $"{ManufacturerType},{base.GetFileFormat()},{_power}%";
        }

        /// <summary>
        /// Returns a string that represents this <see cref="Smartwatch"/>.
        /// </summary>
        /// <returns>A string containing this device's information.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}Power: {_power}\n";
        }

        /// <summary>
        /// Notifies the user that the battery level is low.
        /// </summary>
        public void Notify()
        {
            Console.WriteLine("Energy is low");
        }
    }
}
