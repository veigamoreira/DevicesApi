using DevicesDomain.DTOs;
using DevicesDomain.Models;

namespace DevicesService.Interfaces
{
    public interface IDeviceService
    {
        Task<List<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(Guid id);
        Task<List<Device>> GetByBrandAsync(string brand);
        Task<List<Device>> GetByStateAsync(DeviceState state);
        Task<Device> CreateAsync(DeviceCreateDto dto);
        Task<bool> UpdateAsync(Guid id, DeviceUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
