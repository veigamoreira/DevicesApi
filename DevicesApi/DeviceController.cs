using DevicesDomain.DTOs;
using DevicesDomain.Models;
using DevicesDomain.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace DevicesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        // GET: api/devices
        [HttpGet]
        public async Task<ActionResult<List<DeviceReadDto>>> GetAll()
        {
            var devices = await _service.GetAllAsync();
            return Ok(devices);
        }

        // GET: api/devices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceReadDto>> GetById(Guid id)
        {
            var device = await _service.GetByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);
        }

        // GET: api/devices/brand/{brand}
        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<List<DeviceReadDto>>> GetByBrand(string brand)
        {
            var devices = await _service.GetByBrandAsync(brand);
            return Ok(devices);
        }

        // GET: api/devices/state/{state}
        [HttpGet("state/{state}")]
        public async Task<ActionResult<List<DeviceReadDto>>> GetByState(string state)
        {
            if (!Enum.TryParse<DeviceState>(state, true, out var parsedState))
                return BadRequest(new { error = $"Invalid state: {state}" });

            var devices = await _service.GetByStateAsync(parsedState);
            return Ok(devices);
        }

        // POST: api/devices
        [HttpPost]
        public async Task<ActionResult<DeviceReadDto>> Create(DeviceCreateDto dto)
        {
            try
            {
                var device = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/devices/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceReadDto>> Update(Guid id, DeviceUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/devices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
