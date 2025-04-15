using System.Runtime.Loader;
using System.Text.RegularExpressions;
using task2.Interfaces;

namespace task2
{
    /// <summary>
    /// Manages a collection of <see cref="Device"/> objects and provides methods to manipulate them.
    /// </summary>
    public class DeviceManager : IDeviceManager, IDeviceController
    {
        
        private static List<Device> deviceStorage = new List<Device>();

       
        private IFileManager fileManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceManager"/> class.
        /// </summary>
        /// <param name="filePath">Path to a file from which device data is loaded.</param>
        public DeviceManager(string filePath)
        {
            fileManager = new FIleHandler();
            deviceStorage = fileManager.loadFile(filePath);
        }

        /// <summary>
        /// Adds a device to the internal storage if there is capacity.
        /// </summary>
        /// <param name="device">A <see cref="Device"/> object to be added.</param>
        /// <returns><c>true</c> if the device is successfully added; otherwise, <c>false</c>.</returns>
        public bool addDevice(Device device)
        {
            if (deviceStorage.Count >= 15)
            {
                Console.WriteLine("Storage is full");
            }
            else
            {
                deviceStorage.Add(device);
                Console.WriteLine("Device added");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Finds a device by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the device.</param>
        /// <returns>The <see cref="Device"/> if found, otherwise <c>null</c>.</returns>
        public Device findDevice(string id)
        {
            foreach (Device device in deviceStorage)
            {
                if (device._id == id)
                    return device;
            }

            return null;
        }

        /// <summary>
        /// Removes a device with the specified ID from storage.
        /// </summary>
        /// <param name="id">The unique identifier of the device to remove.</param>
        /// <returns><c>true</c> if the device is successfully removed; otherwise <c>false</c>.</returns>
        public bool removeDevice(string id)
        {
            Device device = findDevice(id);
            if (device != null)
                deviceStorage.Remove(findDevice(id));
            else
            {
                Console.WriteLine("Device not found");
                return false;
            }

            Console.WriteLine("Device removed");
            return true;
        }

        /// <summary>
        /// Edits an existing device by replacing its properties with the properties of the provided device.
        /// </summary>
        /// <param name="device">The updated <see cref="Device"/> data.</param>
        /// <param name="id">The ID of the device to be edited.</param>
        /// <returns><c>true</c> if edit is successful; otherwise <c>false</c>.</returns>
        public bool editDevice(Device device, string id)
        {
            Device oldDevice = findDevice(id);
            if (oldDevice == null)
                return false;
            try
            {
                oldDevice.Edit(device);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Powers on the device with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the device to power on.</param>
        public void turnOn(string id)
        {
            Device device = findDevice(id);
            try
            {
                device.PowerOn();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Powers off the device with the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the device to power off.</param>
        public void turnOff(string id)
        {
            Device device = findDevice(id);
            device.PowerOff();
        }

        /// <summary>
        /// Saves the current device storage to a file.
        /// </summary>
        /// <param name="path">The path to the file where data should be saved.</param>
        /// <returns><c>true</c> if saving was successful; otherwise <c>false</c>.</returns>
        public bool saveStorage(string path)
        {
            return fileManager.saveFile(path, deviceStorage);
        }

        /// <summary>
        /// Returns a string representation of all devices in the internal storage.
        /// </summary>
        /// <returns>A string with device information.</returns>
        public override string ToString()
        {
            string output = "";
            foreach (Device device in deviceStorage)
                output += device.ToString();
            return output;
        }
    }
}
