using DevicesDomain.DTOs;
using DevicesDomain.Models;
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
        public async Task<ActionResult<List<Device>>> GetAll()
        {
            var devices = await _service.GetAllAsync();
            return Ok(devices);
        }

        // GET: api/devices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetById(Guid id)
        {
            var device = await _service.GetByIdAsync(id);
            if (device == null) return NotFound();
            return Ok(device);
        }

        // GET: api/devices/brand/{brand}
        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<List<Device>>> GetByBrand(string brand)
        {
            var devices = await _service.GetByBrandAsync(brand);
            return Ok(devices);
        }

        // GET: api/devices/state/{state}
        [HttpGet("state/{state}")]
        public async Task<ActionResult<List<Device>>> GetByState(DeviceState state)
        {
            var devices = await _service.GetByStateAsync(state);
            return Ok(devices);
        }

        // POST: api/devices
        [HttpPost]
        public async Task<ActionResult<Device>> Create(DeviceCreateDto dto)
        {
            var device = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        // PUT: api/devices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, DeviceUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}
