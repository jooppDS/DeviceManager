namespace task2.Interfaces
{
    public interface IFileManager
    {
        List<Device> LoadFile(string path);
        bool SaveFile(string path, List<Device> deviceStorage);
    }
}