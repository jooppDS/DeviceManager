using task2;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagerRest.Controllers;


public class DeviceManagerController : Controller
{
    private readonly DeviceManager _deviceManager;
    private readonly string _storageFilePath;

    
    public DeviceManagerController()
    {
        _storageFilePath = "C:\\Users\\pc\\RiderProjects\\NET_project\\input.txt";
        _deviceManager = new DeviceManager(_storageFilePath);
    }

    [HttpGet]
    [Route("/DeviceList")]
    public IActionResult GetDeviceList()
    {
        return Ok(_deviceManager.ToString());
    }

    [HttpGet]
    [Route("/DeviceList/{id}")]
    public IActionResult GetDevice(string id)
    {
        var device = _deviceManager.findDevice(id);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(device);
    }

    [HttpPost]
    [Route("/AddDevice")]
    public IActionResult CreateDevice([FromBody] DeviceDto dto)
    {
        Device device = CreateDeviceFromDto(dto);
        if (device == null)
        {
            return BadRequest("Invalid device type.");
        }

        _deviceManager.addDevice(device);
        SaveDeviceList();
        return Ok(device);
    }

    [HttpPut]
    [Route("/UpdateDevice")]
    public IActionResult UpdateDevice([FromBody] DeviceDto dto)
    {
        var oldDevice = _deviceManager.findDevice(dto.Id);

        if (oldDevice == null)
        {
            return NotFound();
        }

        Device newDevice = CreateDeviceFromDto(dto);
        if (newDevice == null)
        {
            return BadRequest("Invalid device type.");
        }

        oldDevice.Edit(newDevice);
        SaveDeviceList();
        return Ok(oldDevice);
    }

    [HttpDelete]
    [Route("/DeleteDevice")]
    public IActionResult DeleteDevice(string id)
    {
        if (_deviceManager.removeDevice(id))
        {
            SaveDeviceList();
            return Ok();
        }
        else
        {
            return NotFound("Device not found");
        }
    }

  
    private Device CreateDeviceFromDto(DeviceDto dto)
    {
        switch (dto.DeviceType)
        {
            case "C":
                return new PersonalComputer(dto.Id, dto.Name, dto.Active ?? false, dto.OS);
            case "ED":
                return new EmbededDevice(dto.Id, dto.Name, dto.Active ?? false, dto.Ip, dto.NetworkName);
            case "SW":
                return new Smartwatch(dto.Id, dto.Name, dto.Active ?? false, dto.Power ?? 0);
            default:
                return null;
        }
    }

  
    private void SaveDeviceList()
    {
        _deviceManager.saveStorage(_storageFilePath);
    }
}
