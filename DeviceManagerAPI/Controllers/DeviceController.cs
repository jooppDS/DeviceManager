using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using DeviceManager.Data;
using DeviceManagerLib;
using System.Text.Json;

namespace DeviceManagerAPI.Controllers
{
    
    
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceRepository _repository;

        public DeviceController(DeviceRepository repository)
        {
            _repository = repository;
        }

        // GET: api/devices
        [HttpGet]
        public IActionResult GetAllDevices()
        {
            var devices = _repository.GetAllDevices();
            return Ok(devices);
        }

        // GET: api/devices/{id}
        [HttpGet("{id}")]
        public IActionResult GetDeviceById(string id)
        {
            var device = _repository.GetDeviceById(id);
            if (device == null)
                return NotFound();
            return Ok(device);
        }

        // POST: api/devices
        [HttpPost]
        public IActionResult CreateDevice([FromBody] JsonElement deviceDto)
        {
            if (!deviceDto.TryGetProperty("deviceType", out var deviceTypeProp))
                return BadRequest("Missing deviceType field.");
            var deviceType = deviceTypeProp.GetString()?.ToLower();
            try
            {
                switch (deviceType)
                {
                    case "personalcomputer":
                    case "pc":
                        var pc = JsonSerializer.Deserialize<PersonalComputer>(deviceDto.GetRawText());
                        if (pc == null || string.IsNullOrWhiteSpace(pc.Id) || string.IsNullOrWhiteSpace(pc.OS))
                            return BadRequest("Invalid PC data.");
                        var pcDevice = new Device { Id = pc.Id, Name = pc.OS, IsEnabled = true };
                        _repository.CreateDevice(pcDevice, pc);
                        return Created($"/api/devices/{pc.Id}", pc);
                    case "embedded":
                    case "embeddeddevice":
                        var emb = JsonSerializer.Deserialize<EmbeddedDevice>(deviceDto.GetRawText());
                        if (emb == null || string.IsNullOrWhiteSpace(emb.Id) || string.IsNullOrWhiteSpace(emb.Ip) || string.IsNullOrWhiteSpace(emb.NetworkName))
                            return BadRequest("Invalid EmbeddedDevice data.");
                        if (!System.Net.IPAddress.TryParse(emb.Ip, out _))
                            return BadRequest("Invalid IP address format.");
                        var embDevice = new Device { Id = emb.Id, Name = emb.NetworkName, IsEnabled = true };
                        _repository.CreateDevice(embDevice, emb);
                        return Created($"/api/devices/{emb.Id}", emb);
                    case "smartwatch":
                        var sw = JsonSerializer.Deserialize<Smartwatch>(deviceDto.GetRawText());
                        if (sw == null || string.IsNullOrWhiteSpace(sw.Id))
                            return BadRequest("Invalid Smartwatch data.");
                        if (sw.Power < 0)
                            return BadRequest("Power must be non-negative.");
                        var swDevice = new Device { Id = sw.Id, Name = "Smartwatch", IsEnabled = true };
                        _repository.CreateDevice(swDevice, sw);
                        return Created($"/api/devices/{sw.Id}", sw);
                    default:
                        return BadRequest("Unknown deviceType.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PUT: api/devices/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDevice(string id, [FromBody] JsonElement deviceDto)
        {
            if (!deviceDto.TryGetProperty("deviceType", out var deviceTypeProp))
                return BadRequest("Missing deviceType field.");
            var deviceType = deviceTypeProp.GetString()?.ToLower();
            try
            {
                switch (deviceType)
                {
                    case "personalcomputer":
                    case "pc":
                        var pc = JsonSerializer.Deserialize<PersonalComputer>(deviceDto.GetRawText());
                        if (pc == null || string.IsNullOrWhiteSpace(pc.Id) || string.IsNullOrWhiteSpace(pc.OS))
                            return BadRequest("Invalid PC data.");
                        var pcDevice = new Device { Id = id, Name = pc.OS, IsEnabled = true };
                        _repository.UpdateDevice(pcDevice, pc);
                        return Ok(pc);
                    case "embedded":
                    case "embeddeddevice":
                        var emb = JsonSerializer.Deserialize<EmbeddedDevice>(deviceDto.GetRawText());
                        if (emb == null || string.IsNullOrWhiteSpace(emb.Id) || string.IsNullOrWhiteSpace(emb.Ip) || string.IsNullOrWhiteSpace(emb.NetworkName))
                            return BadRequest("Invalid EmbeddedDevice data.");
                        if (!System.Net.IPAddress.TryParse(emb.Ip, out _))
                            return BadRequest("Invalid IP address format.");
                        var embDevice = new Device { Id = id, Name = emb.NetworkName, IsEnabled = true };
                        _repository.UpdateDevice(embDevice, emb);
                        return Ok(emb);
                    case "smartwatch":
                        var sw = JsonSerializer.Deserialize<Smartwatch>(deviceDto.GetRawText());
                        if (sw == null || string.IsNullOrWhiteSpace(sw.Id))
                            return BadRequest("Invalid Smartwatch data.");
                        if (sw.Power < 0)
                            return BadRequest("Power must be non-negative.");
                        var swDevice = new Device { Id = id, Name = "Smartwatch", IsEnabled = true };
                        _repository.UpdateDevice(swDevice, sw);
                        return Ok(sw);
                    default:
                        return BadRequest("Unknown deviceType.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // DELETE: api/devices/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDevice(string id)
        {
            try
            {
                _repository.DeleteDevice(id);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}   
