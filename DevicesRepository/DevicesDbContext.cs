using DevicesDomain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DevicesRepository
{
    public class DevicesDbContext : DbContext
    {
        public DevicesDbContext(DbContextOptions<DevicesDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
    }
}
