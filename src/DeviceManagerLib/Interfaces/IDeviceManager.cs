namespace task2.Interfaces
{
    public interface IDeviceManager
    {
        bool AddDevice(Device device);
        Device FindDevice(string id);
        bool RemoveDevice(string id);
        bool EditDevice(Device device, string id);
    }
}