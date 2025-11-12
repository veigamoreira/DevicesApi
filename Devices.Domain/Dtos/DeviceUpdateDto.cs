using DevicesDomain.Models;

namespace DevicesDomain.DTOs
{
    public class DeviceUpdateDto
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public DeviceState? State { get; set; }
    }
}
