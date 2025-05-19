namespace task2
{
    /// <summary>
    /// A factory class for creating <see cref="DeviceManager"/> instances.
    /// </summary>
    public class DeviceManagerFactory
    {
        /// <summary>
        /// Creates a new <see cref="DeviceManager"/> using the provided file path.
        /// </summary>
        /// <param name="path">The file path containing device data.</param>
        /// <returns>A new <see cref="DeviceManager"/> instance.</returns>
        public DeviceManager CreateDM(string path)
        {
            return new DeviceManager(path);
        }
    }
}