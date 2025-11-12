using DevicesDomain.DTOs;
using DevicesDomain.Models;
using DevicesRepository.Interfaces;
using DevicesService.Interfaces;

namespace DevicesService.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Device>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Device?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task<List<Device>> GetByBrandAsync(string brand) => await _repository.GetByBrandAsync(brand);

        public async Task<List<Device>> GetByStateAsync(DeviceState state) => await _repository.GetByStateAsync(state);

        public async Task<Device> CreateAsync(DeviceCreateDto dto)
        {
            if (!Enum.TryParse<DeviceState>(dto.State, ignoreCase: true, out var parsedState))
            {
                throw new ArgumentException($"Invalid State: {dto.State}");
            }

            var device = new Device
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Brand = dto.Brand,
                State = parsedState,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(device);
        }

        public async Task<bool> UpdateAsync(Guid id, DeviceUpdateDto dto)
        {
            var device = await _repository.GetByIdAsync(id);
            if (device == null) return false;

            if (device.State == DeviceState.InUse && (dto.Name != null || dto.Brand != null))
            {
                return false;
            }

            if (dto.Name != null) device.Name = dto.Name;
            if (dto.Brand != null) device.Brand = dto.Brand;
            if (dto.State.HasValue) device.State = dto.State.Value;

            return await _repository.UpdateAsync(device);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var device = await _repository.GetByIdAsync(id);
            if (device == null || device.State == DeviceState.InUse) return false;

            return await _repository.DeleteAsync(id);
        }
    }
}
