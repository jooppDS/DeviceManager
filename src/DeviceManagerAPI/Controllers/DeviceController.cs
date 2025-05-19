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
        [Consumes("application/json", "text/plain")]
        public IActionResult CreateDevice([FromBody] object deviceData)
        {
            try
            {
                if (deviceData is JsonElement jsonElement)
                {
                    return HandleJsonCreate(jsonElement);
                }
                else if (deviceData is string plainText)
                {
                    return HandlePlainTextCreate(plainText);
                }
                return BadRequest("Unsupported content type");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PUT: api/devices/{id}
        [HttpPut("{id}")]
        [Consumes("application/json", "text/plain")]
        public IActionResult UpdateDevice(string id, [FromBody] object deviceData)
        {
            try
            {
                if (deviceData is JsonElement jsonElement)
                {
                    return HandleJsonUpdate(id, jsonElement);
                }
                else if (deviceData is string plainText)
                {
                    return HandlePlainTextUpdate(id, plainText);
                }
                return BadRequest("Unsupported content type");
            }
            catch (ConcurrencyException ex)
            {
                return Conflict(new { message = ex.Message });
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
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Device ID cannot be empty.");
            }

            try
            {
                _repository.DeleteDevice(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the device.", error = ex.Message });
            }
        }

        private IActionResult HandleJsonCreate(JsonElement deviceDto)
        {
            if (!deviceDto.TryGetProperty("deviceType", out var deviceTypeProp))
                return BadRequest("Missing deviceType field.");
            var deviceType = deviceTypeProp.GetString()?.ToLower();
            
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

        private IActionResult HandlePlainTextCreate(string plainText)
        {
            var lines = plainText.Split('\n');
            if (lines.Length < 2)
                return BadRequest("Invalid format. Expected at least 2 lines: device type and device data.");

            var deviceType = lines[0].Trim().ToLower();
            var deviceData = lines[1].Trim();

            switch (deviceType)
            {
                case "personalcomputer":
                case "pc":
                    var pcParts = deviceData.Split(',');
                    if (pcParts.Length < 2)
                        return BadRequest("Invalid PC data format. Expected: id,os");
                    var pc = new PersonalComputer
                    {
                        Id = pcParts[0].Trim(),
                        OS = pcParts[1].Trim()
                    };
                    var pcDevice = new Device { Id = pc.Id, Name = pc.OS, IsEnabled = true };
                    _repository.CreateDevice(pcDevice, pc);
                    return Created($"/api/devices/{pc.Id}", pc);

                case "embedded":
                case "embeddeddevice":
                    var embParts = deviceData.Split(',');
                    if (embParts.Length < 3)
                        return BadRequest("Invalid EmbeddedDevice data format. Expected: id,ip,networkname");
                    if (!System.Net.IPAddress.TryParse(embParts[1].Trim(), out _))
                        return BadRequest("Invalid IP address format.");
                    var emb = new EmbeddedDevice
                    {
                        Id = embParts[0].Trim(),
                        Ip = embParts[1].Trim(),
                        NetworkName = embParts[2].Trim()
                    };
                    var embDevice = new Device { Id = emb.Id, Name = emb.NetworkName, IsEnabled = true };
                    _repository.CreateDevice(embDevice, emb);
                    return Created($"/api/devices/{emb.Id}", emb);

                case "smartwatch":
                    var swParts = deviceData.Split(',');
                    if (swParts.Length < 2)
                        return BadRequest("Invalid Smartwatch data format. Expected: id,power");
                    if (!long.TryParse(swParts[1].Trim(), out long power) || power < 0)
                        return BadRequest("Invalid power value. Must be a non-negative number.");
                    var sw = new Smartwatch
                    {
                        Id = swParts[0].Trim(),
                        Power = power
                    };
                    var swDevice = new Device { Id = sw.Id, Name = "Smartwatch", IsEnabled = true };
                    _repository.CreateDevice(swDevice, sw);
                    return Created($"/api/devices/{sw.Id}", sw);

                default:
                    return BadRequest("Unknown deviceType.");
            }
        }

        private IActionResult HandleJsonUpdate(string id, JsonElement deviceDto)
        {
            if (!deviceDto.TryGetProperty("deviceType", out var deviceTypeProp))
                return BadRequest("Missing deviceType field.");
            var deviceType = deviceTypeProp.GetString()?.ToLower();
            
            switch (deviceType)
            {
                case "personalcomputer":
                case "pc":
                    var pc = JsonSerializer.Deserialize<PersonalComputer>(deviceDto.GetRawText());
                    if (pc == null || string.IsNullOrWhiteSpace(pc.Id) || string.IsNullOrWhiteSpace(pc.OS))
                        return BadRequest("Invalid PC data.");
                    if (pc.Version == null)
                        return BadRequest("Version is required for optimistic concurrency.");
                    var pcDevice = new Device { Id = id, Name = pc.OS, IsEnabled = true, Version = pc.Version };
                    _repository.UpdateDevice(pcDevice, pc);
                    return Ok(pc);
                case "embedded":
                case "embeddeddevice":
                    var emb = JsonSerializer.Deserialize<EmbeddedDevice>(deviceDto.GetRawText());
                    if (emb == null || string.IsNullOrWhiteSpace(emb.Id) || string.IsNullOrWhiteSpace(emb.Ip) || string.IsNullOrWhiteSpace(emb.NetworkName))
                        return BadRequest("Invalid EmbeddedDevice data.");
                    if (!System.Net.IPAddress.TryParse(emb.Ip, out _))
                        return BadRequest("Invalid IP address format.");
                    if (emb.Version == null)
                        return BadRequest("Version is required for optimistic concurrency.");
                    var embDevice = new Device { Id = id, Name = emb.NetworkName, IsEnabled = true, Version = emb.Version };
                    _repository.UpdateDevice(embDevice, emb);
                    return Ok(emb);
                case "smartwatch":
                    var sw = JsonSerializer.Deserialize<Smartwatch>(deviceDto.GetRawText());
                    if (sw == null || string.IsNullOrWhiteSpace(sw.Id))
                        return BadRequest("Invalid Smartwatch data.");
                    if (sw.Power < 0)
                        return BadRequest("Power must be non-negative.");
                    if (sw.Version == null)
                        return BadRequest("Version is required for optimistic concurrency.");
                    var swDevice = new Device { Id = id, Name = "Smartwatch", IsEnabled = true, Version = sw.Version };
                    _repository.UpdateDevice(swDevice, sw);
                    return Ok(sw);
                default:
                    return BadRequest("Unknown deviceType.");
            }
        }

        private IActionResult HandlePlainTextUpdate(string id, string plainText)
        {
            var lines = plainText.Split('\n');
            if (lines.Length < 2)
                return BadRequest("Invalid format. Expected at least 2 lines: device type and device data.");

            var deviceType = lines[0].Trim().ToLower();
            var deviceData = lines[1].Trim();

            switch (deviceType)
            {
                case "personalcomputer":
                case "pc":
                    var pcParts = deviceData.Split(',');
                    if (pcParts.Length < 2)
                        return BadRequest("Invalid PC data format. Expected: id,os");
                    var pc = new PersonalComputer
                    {
                        Id = pcParts[0].Trim(),
                        OS = pcParts[1].Trim()
                    };
                    var pcDevice = new Device { Id = id, Name = pc.OS, IsEnabled = true };
                    _repository.UpdateDevice(pcDevice, pc);
                    return Ok(pc);

                case "embedded":
                case "embeddeddevice":
                    var embParts = deviceData.Split(',');
                    if (embParts.Length < 3)
                        return BadRequest("Invalid EmbeddedDevice data format. Expected: id,ip,networkname");
                    if (!System.Net.IPAddress.TryParse(embParts[1].Trim(), out _))
                        return BadRequest("Invalid IP address format.");
                    var emb = new EmbeddedDevice
                    {
                        Id = embParts[0].Trim(),
                        Ip = embParts[1].Trim(),
                        NetworkName = embParts[2].Trim()
                    };
                    var embDevice = new Device { Id = id, Name = emb.NetworkName, IsEnabled = true };
                    _repository.UpdateDevice(embDevice, emb);
                    return Ok(emb);

                case "smartwatch":
                    var swParts = deviceData.Split(',');
                    if (swParts.Length < 2)
                        return BadRequest("Invalid Smartwatch data format. Expected: id,power");
                    if (!long.TryParse(swParts[1].Trim(), out long power) || power < 0)
                        return BadRequest("Invalid power value. Must be a non-negative number.");
                    var sw = new Smartwatch
                    {
                        Id = swParts[0].Trim(),
                        Power = power
                    };
                    var swDevice = new Device { Id = id, Name = "Smartwatch", IsEnabled = true };
                    _repository.UpdateDevice(swDevice, sw);
                    return Ok(sw);

                default:
                    return BadRequest("Unknown deviceType.");
            }
        }
    }
}   

