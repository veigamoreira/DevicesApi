using DevicesDomain.Models;

namespace DevicesRepository.Interfaces
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(Guid id);
        Task<List<Device>> GetByBrandAsync(string brand);
        Task<List<Device>> GetByStateAsync(DeviceState state);
        Task<Device> CreateAsync(Device device);
        Task<bool> UpdateAsync(Device device);
        Task<bool> DeleteAsync(Guid id);
    }
}
