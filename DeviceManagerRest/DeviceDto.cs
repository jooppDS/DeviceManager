namespace DeviceManagerRest;

public class DeviceDto
{
    private string? _deviceType;
    private string? _id;
    private string? _name;
    private bool? _active;
    private string? _ip;
    private string? _networkName;
    private string? _os;
    private long? _power;

    
    public string? DeviceType
    {
        get { return _deviceType; }
        set { _deviceType = value; }
    }

    public string? Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string? Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public bool? Active
    {
        get { return _active; }
        set { _active = value; }
    }

    public string? Ip
    {
        get { return _ip; }
        set { _ip = value; }
    }

    public string? NetworkName
    {
        get { return _networkName; }
        set { _networkName = value; }
    }

    public string? OS
    {
        get { return _os; }
        set { _os = value; }
    }

    public long? Power
    {
        get { return _power; }
        set { _power = value; }
    }
    
    
}