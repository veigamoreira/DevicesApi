using DevicesDomain.Models;
using DevicesRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevicesRepository.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DevicesDbContext _context;

        public DeviceRepository(DevicesDbContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetAllAsync() =>
            await _context.Devices.ToListAsync();

        public async Task<Device?> GetByIdAsync(Guid id) =>
            await _context.Devices.FindAsync(id);

        public async Task<List<Device>> GetByBrandAsync(string brand) =>
            await _context.Devices
                .Where(d => d.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

        public async Task<List<Device>> GetByStateAsync(DeviceState state) =>
            await _context.Devices
                .Where(d => d.State == state)
                .ToListAsync();

        public async Task<Device> CreateAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null || device.State == DeviceState.InUse) return false;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
