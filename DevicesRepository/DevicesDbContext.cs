using DevicesDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevicesRepository
{
    public class DevicesDbContext : DbContext
    {
        public DevicesDbContext(DbContextOptions<DevicesDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
    }
}
