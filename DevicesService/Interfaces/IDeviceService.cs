using DevicesDomain.DTOs;
using DevicesDomain.Models;

public interface IDeviceService
{
    Task<List<DeviceReadDto>> GetAllAsync();
    Task<DeviceReadDto?> GetByIdAsync(Guid id);
    Task<List<DeviceReadDto>> GetByBrandAsync(string brand);
    Task<List<DeviceReadDto>> GetByStateAsync(DeviceState state);
    Task<DeviceReadDto> CreateAsync(DeviceCreateDto dto);
    Task<DeviceReadDto?> UpdateAsync(Guid id, DeviceUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
