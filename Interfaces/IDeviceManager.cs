namespace task2.Interfaces;

public interface IDeviceManager
{
    bool addDevice(Device device);
    Device findDevice(string id);
    bool removeDevice(string id);
    bool editDevice(Device device, string id);
}