namespace task2.Interfaces;

public interface IFileManager
{
    List<Device> loadFile(string path);
    bool saveFile(string path, List<Device> deviceStorage);
}