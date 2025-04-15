using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using task2;

namespace DeviceManagerAPI.Controllers
{
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceManager _deviceManager;
        private readonly string _storageFilePath;


        public DeviceController()
        {
            _storageFilePath = Path.Combine(AppContext.BaseDirectory, "input.txt");
            _deviceManager = new DeviceManager(_storageFilePath);
        } 
        
        [HttpGet]
        [Route("devices")]
        public IActionResult GetDeviceList() // Needed IActionResult to get formated json output
        {
            return Ok(_deviceManager.ToString());
        }

        [HttpGet]
        [Route("devices/{id}")]
        public IResult GetDevice(string id)
        {
            var device = _deviceManager.FindDevice(id);
            if (device == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(device);
        }
    
        [HttpPost]
        [Route("computer")]
        public IResult CreatePC([FromBody] PersonalComputer computer)
        {
            if (computer == null)
                return Results.BadRequest("Invalid device type.");
            _deviceManager.AddDevice(computer);
            SaveDeviceList();
            return Results.Ok(computer);
        }
        
        [HttpPost]
        [Route("watch")]
        public IResult CreateWatch([FromBody] Smartwatch watch)
        {
            if (watch == null)
                return Results.BadRequest("Invalid device type.");
            _deviceManager.AddDevice(watch);
            SaveDeviceList();
            return Results.Ok(watch);
        }
        
        [HttpPost]
        [Route("embeddedDevice")]
        public IResult CreatePC([FromBody] EmbededDevice embededDevice)
        {
            if (embededDevice == null)
                return Results.BadRequest("Invalid device type.");
            _deviceManager.AddDevice(embededDevice);
            SaveDeviceList();
            return Results.Ok(embededDevice);
        }
    
        
        [HttpPut]
        [Route("computer/{id}")]
        public IResult UpdateDevice(String id, [FromBody] PersonalComputer computer)
        {
            if (computer == null)
                return Results.BadRequest("Invalid device");
            var oldDevice = _deviceManager.FindDevice(id);
            if (oldDevice == null)
                return Results.NotFound();
            oldDevice.Edit(computer);
            SaveDeviceList();
            return Results.Ok(oldDevice);
        }
        
        [HttpPut]
        [Route("watch/{id}")]
        public IResult UpdateDevice(String id, [FromBody] Smartwatch watch)
        {
            if (watch == null)
                return Results.BadRequest("Invalid device");
            var oldDevice = _deviceManager.FindDevice(id);
            if (oldDevice == null)
                return Results.NotFound();
            oldDevice.Edit(watch);
            SaveDeviceList();
            return Results.Ok(oldDevice);
        }
        
        [HttpPut]
        [Route("embeddedDevice/{id}")]
        public IResult UpdateDevice(String id, [FromBody] EmbededDevice embededDevice)
        {
            if (embededDevice == null)
                return Results.BadRequest("Invalid device");
            var oldDevice = _deviceManager.FindDevice(id);
            if (oldDevice == null)
                return Results.NotFound();
            oldDevice.Edit(embededDevice);
            SaveDeviceList();
            return Results.Ok(oldDevice);
        }
        
        
    
        [HttpDelete]
        [Route("device/{id}")]
        public IResult DeleteDevice(string id)
        {
            if (_deviceManager.RemoveDevice(id))
            {
                SaveDeviceList();
                return Results.Ok(200);
            }
            return Results.NotFound("Device not found");
        }
    
        
    
    
        private void SaveDeviceList()
        {
            _deviceManager.SaveStorage(_storageFilePath);
        }
        }
}   
