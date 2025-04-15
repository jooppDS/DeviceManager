/*
namespace task2.test;

using Xunit;

public class DeviceManagerTest
{

    

    [Fact]
    public void AddTest() {
        
        Devicemanager a = new Devicemanager();
        
        Assert.True(a.addDevice(new Smartwatch(1,"Example1",true, 20)));
        Assert.True(a.addDevice(new PersonalComputer(2,"Example2",false, "Linux")));
        Assert.True(a.addDevice(new EmbededDevice(3,"Example3",true, "255.255.255.255","MD Ltd.Wifi-1")));
        Assert.True(a.addDevice(new Smartwatch(4,"Example4",true, 20)));
        Assert.True(a.addDevice(new Smartwatch(5,"Example1",true, 20)));
        Assert.True(a.addDevice(new PersonalComputer(6,"Example2",false, "Linux")));
        Assert.True(a.addDevice(new EmbededDevice(7,"Example3",true, "255.255.255.255","MD Ltd.Wifi-1")));
        Assert.True(a.addDevice(new Smartwatch(8,"Example4",true, 20)));
        Assert.True(a.addDevice(new Smartwatch(9,"Example1",true, 20)));
        Assert.True(a.addDevice(new PersonalComputer(10,"Example2",false, "Linux")));
        Assert.True(a.addDevice(new EmbededDevice(11,"Example3",true, "255.255.255.255","MD Ltd.Wifi-1")));
        Assert.True(a.addDevice(new Smartwatch(12,"Example4",true, 20)));
        Assert.True(a.addDevice(new Smartwatch(13,"Example1",true, 20)));
        Assert.True(a.addDevice(new PersonalComputer(14,"Example2",false, "Linux")));
        Assert.True(a.addDevice(new EmbededDevice(15,"Example3",true, "255.255.255.255","MD Ltd.Wifi-1")));
        Assert.False(a.addDevice(new Smartwatch(16,"Example4",true, 20)));
    }

    [Fact]

    public void RemoveTest() {
        Devicemanager a = new Devicemanager();
        
        Assert.True(a.addDevice(new Smartwatch(1,"Example1",true, 20)));
        Assert.True(a.addDevice(new PersonalComputer(2,"Example2",false, "Linux")));
        
        Assert.True(a.removeDevice(1));
        Assert.True(a.removeDevice(2));
        Assert.False(a.removeDevice(0));
    }

    [Fact]
    public void EditTest() {
        Devicemanager a = new Devicemanager();
        PersonalComputer device = new PersonalComputer(1, "PC1", true, "Windows");
        a.addDevice(device);
        PersonalComputer updatedDevice = new PersonalComputer(1, "PC2", false, "Linux");
        
        Assert.True(a.editDevice(updatedDevice, 1));
        Assert.False(a.editDevice(updatedDevice, 2));
    }


    [Fact]
    public void PowerOffTest() {
        
        Devicemanager a = new Devicemanager();
        PersonalComputer device = new PersonalComputer(1, "PC1", true, "Windows");
        a.addDevice(device);

        a.turnOff(1);

        Assert.False(device._active);
    }
    
    
    [Fact]
    public void PowerOnTest() {
        Devicemanager a = new Devicemanager();
        PersonalComputer device = new PersonalComputer(1, "PC1", false, "Windows");
        a.addDevice(device);

        a.turnOn(1);

        Assert.True(device._active);
    }

    [Fact]
    public void DisplayTest()
    {
        Devicemanager a = new Devicemanager();
        PersonalComputer device = new PersonalComputer(1, "PC1", false, "Windows");
        a.addDevice(device);

        string result = a.ToString();

        Assert.Contains("PC1", result);
    }


    [Fact]

    public void SavingTest() {
        Devicemanager a = new Devicemanager();
        PersonalComputer device = new PersonalComputer(1, "PC1", true, "Windows");
        a.addDevice(device);
        string filename = "test_file.txt";
        if (File.Exists(filename))
            File.Delete(filename);
            
        a.saveStorage(filename);

        Assert.True(File.Exists(filename));
        File.Delete(filename);
    }
}

*/