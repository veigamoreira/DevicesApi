using DevicesDomain.Models;

namespace DevicesDomain.DTOs
{
    public class DeviceCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public DeviceState State { get; set; }
    }
}
